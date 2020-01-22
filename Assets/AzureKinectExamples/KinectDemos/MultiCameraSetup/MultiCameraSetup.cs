using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.rfilkov.kinect;

namespace com.rfilkov.components
{
    /// <summary>
    /// MultiCameraSetup automarically estimates the positions and rotations of the sensors in a multi-camera setup.
    /// </summary>
    public class MultiCameraSetup : MonoBehaviour
    {
        [Tooltip("Index of the player, tracked by this component. 0 means the 1st player, 1 - the 2nd one, 2 - the 3rd one, etc.")]
        private int playerIndex = 0;

        //[Tooltip("Number of poses used for the multi-camera setup.")]
        //public int numberOfPoses = 100;

        [Tooltip("Prefab used to display the user mesh, as perceived by each camera.")]
        public GameObject userMeshPrefab;

        [Tooltip("Progress bar to show overall calibration progress.")]
        public UnityEngine.UI.Scrollbar progressBar;

        [Tooltip("UI text for displaying the progress percentage.")]
        public UnityEngine.UI.Text progressText;

        [Tooltip("UI text used for camera information messages.")]
        public UnityEngine.UI.Text camInfoText;


        // reference to KinectManager
        private KinectManager kinectManager = null;

        // initial user orientation
        private Quaternion initialUserRot = Quaternion.identity;

        // number of sensors
        private int numSensors = 0;

        // multi camera pose
        private KinectInterop.MultiCameraPose multiCamPose = new KinectInterop.MultiCameraPose();

        // user mesh for each camera
        private GameObject[] cameraUserMesh = null;

        // constants
        private const int MAX_SAVED_POSES = 100;
        private const float TIME_BETWEEN_PROBES = 0.2f;

        // saved camera positions & rotations
        private List<Vector3>[] savedCamPos = null;
        private List<Quaternion>[] savedCamRot = null;

        // source camera pose
        private Vector3 refCameraPos = Vector3.zero;
        private Quaternion refCameraRot = Quaternion.identity;

        // user pose in source camera space
        private Vector3 refUserPos = Vector3.zero;
        private Quaternion refUserRot = Quaternion.identity;

        private Vector3 userBodyPos = Vector3.zero;
        private Quaternion userBodyRot = Quaternion.identity;

        // camera and user pose matrices
        private Matrix4x4 srcCameraMatrix = Matrix4x4.identity;
        private Matrix4x4 srcUserMatrix = Matrix4x4.identity;

        // whether to stop the co-routine or not
        private bool bStopCoroutine = false;

        //// user world pos & rot
        //private Vector3[] userWorldPos = null;
        //private Quaternion[] userWorldRot = null;


        void Start()
        {
            kinectManager = KinectManager.Instance;
            numSensors = kinectManager ? kinectManager.GetSensorCount() : 0;

            if (numSensors > 1)
            {
                ShowDebugMessage(numSensors + " sensors found. Starting multi-camera setup...");
            }
            else
            {
                ShowErrorMessage(numSensors + " sensors found. No need for multi-camera setup.");
                return;
            }

            // user meshes per camera
            cameraUserMesh = new GameObject[numSensors];

            // init camera pos & rot
            savedCamPos = new List<Vector3>[numSensors];
            savedCamRot = new List<Quaternion>[numSensors];

            // init multi-camera pose
            multiCamPose.version = 1;
            multiCamPose.camPose = new KinectInterop.CameraPose[numSensors];
            multiCamPose.settings = new string[numSensors];

            for (int i = 0; i < numSensors; i++)
            {
                KinectInterop.SensorData sensorData = kinectManager.GetSensorData(i);

                if (sensorData != null && sensorData.sensorInterface != null)
                {
                    multiCamPose.camPose[i] = new KinectInterop.CameraPose();
                    multiCamPose.camPose[i].sensorType = (int)sensorData.sensorInterface.GetSensorPlatform();
                    multiCamPose.camPose[i].sensorIndex = ((DepthSensorBase)sensorData.sensorInterface).deviceIndex;
                    multiCamPose.camPose[i].sensorId = sensorData.sensorInterface.GetSensorDeviceId();

                    DepthSensorBase.BaseSensorSettings settings = sensorData.sensorInterface.GetSensorSettings(null);
                    multiCamPose.settings[i] = JsonUtility.ToJson(settings);

                    if (i != 0)
                    {
                        // reset poses of all non-first cameras
                        sensorData.sensorInterface.SetSensorToWorldMatrix(Vector3.zero, Quaternion.identity, true);
                    }
                }

                savedCamPos[i] = new List<Vector3>();
                savedCamRot[i] = new List<Quaternion>();

                if (userMeshPrefab != null)
                {
                    cameraUserMesh[i] = Instantiate(userMeshPrefab, transform);
                    cameraUserMesh[i].name = ((KinectInterop.DepthSensorPlatform)multiCamPose.camPose[i].sensorType).ToString() + multiCamPose.camPose[i].sensorIndex + "Mesh";

                    UserMeshRendererGpu meshRenderer = cameraUserMesh[i].GetComponent<UserMeshRendererGpu>();
                    if (meshRenderer != null)
                        meshRenderer.sensorIndex = i;
                }
            }

            //userWorldPos = new Vector3[numSensors];
            //userWorldRot = new Quaternion[numSensors];

            // initial user rotation
            initialUserRot = Quaternion.Euler(0f, 180f, 0f);  // always mirrored

            // start co-routine
            bStopCoroutine = false;
            StartCoroutine(EstimateCameraPoses());
        }


        void OnDestroy()
        {
            // stop all co-routines
            bStopCoroutine = true;
            StopAllCoroutines();
        }


        // periodically estimates the camera poses
        private IEnumerator EstimateCameraPoses()
        {
            while (!bStopCoroutine)
            {
                bool bSingleUser = CheckForSingleUser();
                int numSamples = 0;

                if (kinectManager && kinectManager.IsInitialized() && numSensors > 1 && bSingleUser)
                {
                    for (int i = 0; i < numSensors; i++)
                    {
                        KinectInterop.SensorData sensorData = kinectManager.GetSensorData(i);

                        if (i == 0)
                        {
                            // first camera
                            Transform sensorTrans = kinectManager.GetSensorTransform(0);
                            refCameraPos = sensorTrans.position;
                            refCameraRot = sensorTrans.rotation;

                            // src camera matrix
                            srcCameraMatrix = Matrix4x4.TRS(refCameraPos, refCameraRot, Vector3.one);

                            // src user pose
                            if (GetUserPose(sensorData, playerIndex, ref refUserPos, ref refUserRot))
                            {
                                // src user matrix
                                srcUserMatrix = Matrix4x4.TRS(refUserPos, refUserRot, Vector3.one);

                                // save current camera pose
                                SaveSensorPose(i, refCameraPos, refCameraRot);
                            }
                        }
                        else
                        {
                            // other camera
                            if (GetUserPose(sensorData, playerIndex, ref userBodyPos, ref userBodyRot))
                            {
                                // dst user matrix
                                Matrix4x4 dstUserMatrix = Matrix4x4.TRS(userBodyPos, userBodyRot, Vector3.one);

                                // dst camera matrix
                                Matrix4x4 dstCameraMatrix = srcCameraMatrix * srcUserMatrix * dstUserMatrix.inverse;

                                // est camera position & rotation
                                Vector3 estCameraPos = dstCameraMatrix.GetColumn(3);
                                Quaternion estCameraRot = dstCameraMatrix.rotation;

                                // save current camera pose
                                SaveSensorPose(i, estCameraPos, estCameraRot);
                            }
                        }
                    }

                    System.DateTime dtNow = System.DateTime.UtcNow;
                    multiCamPose.estimatedAtTime = dtNow.Ticks;
                    multiCamPose.estimatedDateTime = dtNow.ToShortDateString() + " " + dtNow.ToShortTimeString();

                    // show progress
                    numSamples = GetNumberOfSamples();
                    float fProgress = (float)numSamples / (float)MAX_SAVED_POSES;

                    if (progressBar != null)
                    {
                        progressBar.size = fProgress;
                    }

                    if (progressText != null)
                    {
                        progressText.text = string.Format("{0:F0}%", fProgress * 100f);
                    }

                    // show sensor info
                    ShowCamPosesInfo(fProgress);
                }

                if (numSensors > 1 && !bSingleUser)
                {
                    if (camInfoText != null)
                    {
                        camInfoText.text = "Only one person should stay visible to all sensors until the calibration completes.";
                    }
                }

                if (numSamples < MAX_SAVED_POSES)
                {
                    // wait between samples
                    yield return new WaitForSeconds(TIME_BETWEEN_PROBES);
                }
                else
                {
                    // save the multi-camera config
                    string multiCamJson = JsonUtility.ToJson(multiCamPose, true);
                    KinectInterop.SaveTextFile(KinectInterop.MULTI_CAM_CONFIG_FILE_NAME, multiCamJson);

                    if (camInfoText != null)
                    {
                        camInfoText.text = "Saved multi-camera configuration. Calibration finished!";
                    }

                    yield break;
                }

            }
        }


        // checks for single user, visible to all sensors
        private bool CheckForSingleUser()
        {
            for (int i = 0; i < numSensors; i++)
            {
                KinectInterop.SensorData sensorData = kinectManager.GetSensorData(i);
                if (sensorData.trackedBodiesCount != 1)
                    return false;
            }

            return true;
        }


        // returns the minimum number of samples, for all sensors
        private int GetNumberOfSamples()
        {
            int minSamples = MAX_SAVED_POSES;

            for (int i = 0; i < numSensors; i++)
            {
                minSamples = Mathf.Min(minSamples, savedCamPos[i].Count);
                minSamples = Mathf.Min(minSamples, savedCamRot[i].Count);
            }

            return minSamples;
        }


        // gets the position and rotation of the given user
        private bool GetUserPose(KinectInterop.SensorData sensorData, int uIndex, ref Vector3 userPos, ref Quaternion userRot)
        {
            if (sensorData != null && sensorData.trackedBodiesCount > uIndex)
            {
                KinectInterop.BodyData bodyData = sensorData.alTrackedBodies[uIndex];
                Vector3 spaceScale = sensorData.sensorSpaceScale;

                KinectInterop.TrackingState trackingState = bodyData.joint != null && bodyData.joint.Length > 0 ? bodyData.joint[0].trackingState : KinectInterop.TrackingState.NotTracked;
                if (trackingState == KinectInterop.TrackingState.Tracked || trackingState == KinectInterop.TrackingState.HighConf)
                {
                    userPos = new Vector3(bodyData.kinectPos.x * spaceScale.x, bodyData.kinectPos.y * spaceScale.y, bodyData.kinectPos.z);
                    userRot = initialUserRot * bodyData.mirroredRotation;
                }

                return true;
            }

            return false;
        }


        // saves the currently estimated sensor pose
        private void SaveSensorPose(int sIndex, Vector3 sPos, Quaternion sRot)
        {
            while (savedCamPos[sIndex].Count >= MAX_SAVED_POSES)
                savedCamPos[sIndex].RemoveAt(0);
            savedCamPos[sIndex].Add(sPos);

            while (savedCamRot[sIndex].Count >= MAX_SAVED_POSES)
                savedCamRot[sIndex].RemoveAt(0);
            savedCamRot[sIndex].Add(sRot);

            // estimate the average camera pose
            //multiCamPose.camPose[sIndex].position = sPos;
            //multiCamPose.camPose[sIndex].rotation = sRot.eulerAngles;

            if (savedCamPos[sIndex].Count == savedCamRot[sIndex].Count)
            {
                Vector3 camPosSum = Vector3.zero;
                Vector4 camRotSum = Vector4.zero;

                int numSavedPR = savedCamPos[sIndex].Count;
                for (int i = 0; i < numSavedPR; i++)
                {
                    camPosSum += savedCamPos[sIndex][i];
                    KinectInterop.SumUpQuaternions(ref camRotSum, savedCamRot[sIndex][i], savedCamRot[sIndex][0]);
                }

                multiCamPose.camPose[sIndex].position = camPosSum / (float)numSavedPR;
                multiCamPose.camPose[sIndex].rotation = KinectInterop.AverageQuaternions(camRotSum, numSavedPR).eulerAngles;

                if(cameraUserMesh[sIndex] != null)
                {
                    cameraUserMesh[sIndex].transform.position = multiCamPose.camPose[sIndex].position;
                    cameraUserMesh[sIndex].transform.rotation = Quaternion.Euler(multiCamPose.camPose[sIndex].rotation);
                }
            }
        }


        // show the camera poses info on screen
        private void ShowCamPosesInfo(float fProgress)
        {
            System.Text.StringBuilder sbInfo = new System.Text.StringBuilder();

            for (int i = 0; i < numSensors; i++)
            {
                sbInfo.AppendFormat("Camera {0} position: {1}, rotation: {2}", i, multiCamPose.camPose[i].position, multiCamPose.camPose[i].rotation);
                sbInfo.AppendLine();
            }

            //Debug.Log(sbInfo.ToString());
            if (camInfoText != null)
            {
                camInfoText.text = sbInfo.ToString();
            }
        }


        // displays debug message
        private void ShowDebugMessage(string sMsg)
        {
            Debug.Log(sMsg);

            if (camInfoText != null)
            {
                camInfoText.text = sMsg;
            }
        }

        // displays error message
        private void ShowErrorMessage(string sMsg)
        {
            Debug.LogError(sMsg);

            if (camInfoText != null)
            {
                camInfoText.text = sMsg;
            }
        }

    }
}
