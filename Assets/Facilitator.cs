using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Facilitator : MonoBehaviour
{
    private GameObject parentObject;
    private GameObject[] objects4Collison;
    private GameObject wristLeftUpper;
    private GameObject elbowLeft;
    private GameObject wristLeftLower;
    private GameObject wristRightUpper;
    private GameObject elbowRight;
    private GameObject wristRightLower;

    private bool isInit;
    private bool isStep_Raise_both_Elbows_to_Shoulder_Level;

    void Awake()
    {
        this.wristLeftUpper   = GameObject.Find("WRIST_LEFT_UPPER");
        this.elbowLeft        = GameObject.Find("ELBOW_LEFT");
        this.wristLeftLower   = GameObject.Find("WRIST_LEFT_LOWER");
        this.wristRightUpper  = GameObject.Find("WRIST_RIGHT_UPPER");
        this.elbowRight       = GameObject.Find("ELBOW_RIGHT");
        this.wristRightLower  = GameObject.Find("WRIST_RIGHT_LOWER");

        // Get the parent-object
        this.parentObject = GameObject.Find("Objects4Collision");

        // Get all child-objects
        foreach (Transform childTransform in parentObject.transform)
        {
            childTransform.gameObject.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        this.isInit=true;
        this.isStep_Raise_both_Elbows_to_Shoulder_Level=true;   
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isStep_Raise_both_Elbows_to_Shoulder_Level) {
            raise_both_elbows_to_shoulder_level();

            // 両肘が肩の高さまで上がったら、抜ける
        }

        if (){
            // 両肘が肩の高さまで上がっているかは常にチェック
        }
    }

    void raise_both_elbows_to_shoulder_level(){
        if (isInit) {
            this.elbowLeft.SetActive(true);
            this.elbowRight.SetActive(true);

            isInit=false;
        }
        
        bool isCollision4EL = elbowLeft.GetComponent<DetectCollision4E_L>().isCollision;
        bool isCollision4ER = elbowRight.GetComponent<DetectCollision4E_R>().isCollision;
        if () {

        }
    }
}
