﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Facilitator4RL_Elbowless : MonoBehaviour
{
    private GameObject userModel;
    private GameObject gestureListener;
    private GameObject kinectController;

    private GameObject parentObject;
    private GameObject[] objects4Collison;
    private GameObject handLeftUpper;
    private GameObject handLeftDiagUpper;
    private GameObject handLeftMiddle;
    private GameObject handLeftDiagLower;
    private GameObject handLeftLower;

    private GameObject handRightUpper;
    private GameObject handRightDiagUpper;
    private GameObject handRightMiddle;
    private GameObject handRightDiagLower;
    private GameObject handRightLower;

    private GameObject guideText;
    private GameObject countText;
    private GameObject countGauge;
    private GameObject scoreText;
    private GameObject adviceLabel;
    private GameObject adviceText;

    private GameObject goodEffect;
    private GameObject badEffect;
    private GameObject finishEffect;

    public static int maxTimes;
    private int remainingTimes;
    public static int currentScore;

    private bool isFinishedRehabilitation;
    private bool isInit;
    private bool isStep_Raise_Hands;
    private bool isStep_Lower_Hands;
    private bool isActive4Advice;
    private bool isInitRehabilitation;
    private bool isClear4H_LR_D_U;
    private bool isClear4H_LR_M;
    private bool isClear4H_LR_D_L;
    private bool isClear4HAND;

    private AudioSource audioSource;
    public AudioClip goodAudio, badAudio, finishAudio;
    public AudioClip doAudio, reAudio, miAudio;

    // Added by Kawakami
    private int exerciseNum;

    void Awake()
    {
        userModel          = GameObject.Find("User_back");
        gestureListener    = GameObject.Find("GestureListener");
        kinectController   = GameObject.Find("KinectController");

        handLeftUpper      = GameObject.Find("HAND_LEFT_UPPER");
        handLeftDiagUpper  = GameObject.Find("HAND_LEFT_DIAG_UPPER");
        handLeftMiddle     = GameObject.Find("HAND_LEFT_MIDDLE");
        handLeftDiagLower  = GameObject.Find("HAND_LEFT_DIAG_LOWER");
        handLeftLower      = GameObject.Find("HAND_LEFT_LOWER");

        handRightUpper     = GameObject.Find("HAND_RIGHT_UPPER");
        handRightDiagUpper = GameObject.Find("HAND_RIGHT_DIAG_UPPER");
        handRightMiddle    = GameObject.Find("HAND_RIGHT_MIDDLE");
        handRightDiagLower = GameObject.Find("HAND_RIGHT_DIAG_LOWER");
        handRightLower     = GameObject.Find("HAND_RIGHT_LOWER");

        guideText       = GameObject.Find("Guide");
        countText       = GameObject.Find("Count");
        countGauge      = GameObject.Find("CountGauge");
        scoreText       = GameObject.Find("Score");
        adviceLabel     = GameObject.Find("AdviceLabel");
        adviceText      = GameObject.Find("AdviceText");

        goodEffect      = GameObject.Find("GoodEffect");
        badEffect       = GameObject.Find("BadEffect");
        finishEffect    = GameObject.Find("FinishEffect");

        // Get the parent-object
        parentObject    = GameObject.Find("Objects4Collision");

        // Get all child-objects
        foreach (Transform childTransform in parentObject.transform)
        {
            childTransform.gameObject.SetActive(false);
        }

        isFinishedRehabilitation = false;
        isInit = true; 
        isStep_Raise_Hands = true;
        isStep_Lower_Hands = false;
        isActive4Advice = false;
        isInitRehabilitation = true;
        isClear4H_LR_D_U    = false;
        isClear4H_LR_M      = false;
        isClear4H_LR_D_L    = false;
        isClear4HAND        = false;

        countGauge.SetActive(false);
        countGauge.GetComponent<Image>().fillAmount = 1.0f;
        countText.SetActive(false);

        // Added by Kawakami
        exerciseNum = (int)TitleSceneManager.GetCurrentExerciseNum();
        maxTimes = exerciseNum;
        remainingTimes = maxTimes;
        scoreText.SetActive(false);
        currentScore = 0;
        adviceLabel.SetActive(false);

        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        userModel.SetActive(true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isFinishedRehabilitation) {

            if (isStep_Raise_Hands) {
                Raise_Hands();

            } else if (isStep_Lower_Hands) {
                Lower_Hands();
            } // end if

            // Display remaining times
            countText.GetComponent<Text>().text = remainingTimes.ToString();

            // Update count gauge
            countGauge.GetComponent<Image>().fillAmount = (float)remainingTimes / (float)maxTimes;

            // Display current score
            scoreText.GetComponent<Text>().text = "Score: " + currentScore;

            // To the result scene
            if (remainingTimes == 0) {
                isFinishedRehabilitation = true;

                // Display finish message
                // guideText.GetComponent<Text>().fontSize = 150;
                guideText.GetComponent<Text>().text = "";

                // Finish effect and audio
                finishEffect.GetComponent<Emit>().IsEmit = true;
                audioSource.PlayOneShot(finishAudio);

                // Added by Kawakami 2/6
                // Save current score as Exp
                int expCurValue = PlayerPrefs.GetInt("Exp") + currentScore;
                PlayerPrefs.SetInt("Exp", expCurValue);

                // To the result scene
                Invoke("LoadResultScene", 5f);
            } // end if

        } // end if

    } // end func

    private void Raise_Hands(){
        // Display guide text
        if (remainingTimes >= exerciseNum-2)
            DisplayText(guideText, "RAISE your forearms!");
        else
            DisplayText(guideText, "RAISE");

        if (isInitRehabilitation) {
            // Display UI(count, count gauge and score)
            countText.SetActive(true);
            countGauge.SetActive(true);
            scoreText.SetActive(true);

            // Display only the HAND_LR_UPPER objects
            if (isInit) {
                isInit = false;
                handLeftUpper.SetActive(true);
                handRightUpper.SetActive(true);
            } // end if

            // Detect collision for HAND_LR_UPPER
            DetectCollision4HAND_LR_UPPER();

        } else {

            // Display objects for collision detection (Visualize the correct motion)
            if (isInit) {
                isInit = false;
                ResetIsClear();
                handLeftDiagLower.SetActive(true);
                handRightDiagLower.SetActive(true);
                handLeftMiddle.SetActive(true);
                handRightMiddle.SetActive(true);
                handLeftDiagUpper.SetActive(true);
                handRightDiagUpper.SetActive(true);
                handLeftUpper.SetActive(true);
                handRightUpper.SetActive(true);
            } // end if

            DetectCollision4HAND_LR_DIAG_LOWER();

            DetectCollision4HAND_LR_MIDDLE();
            
            DetectCollision4HAND_LR_DIAG_UPPER();

            DetectCollision4HAND_LR_UPPER();

        } // end if
    } // end Raise_Hands()

    private void Lower_Hands(){
        // Display guide text
        if (remainingTimes >= exerciseNum-3)
            DisplayText(guideText, "LOWER your forearms!");
        else
            DisplayText(guideText, "LOWER");

        // Display objects for collision detection (Visualize the correct motion)
        if (isInit) {
            isInit = false;
            ResetIsClear();
            handLeftDiagUpper.SetActive(true);
            handRightDiagUpper.SetActive(true);
            handLeftMiddle.SetActive(true);
            handRightMiddle.SetActive(true);
            handLeftDiagLower.SetActive(true);
            handRightDiagLower.SetActive(true);
            handLeftLower.SetActive(true);
            handRightLower.SetActive(true);
        } // end if

        DetectCollision4HAND_LR_DIAG_UPPER();

        DetectCollision4HAND_LR_MIDDLE();

        DetectCollision4HAND_LR_DIAG_LOWER();
 
        DetectCollision4HAND_LR_LOWER();

    } // end Lower_Hands()

    private void DetectCollision4HAND_LR_UPPER() {
        bool isCollision4H_L_U = handLeftUpper.GetComponent<DetectCollision4H_L_U>().isCollision4HandLT;
        bool isCollision4H_R_U = handRightUpper.GetComponent<DetectCollision4H_R_U>().isCollision4HandRT;
        if (isCollision4H_L_U && isCollision4H_R_U) {
            // Count the number of times
            if (!isInitRehabilitation) remainingTimes -= 1;

            // Apply collisions
            handLeftUpper.GetComponent<DetectCollision4H_L_U>().isCollision4HandLT = false;
            handLeftUpper.GetComponent<Renderer>().material.color = Color.yellow;
            handRightUpper.GetComponent<DetectCollision4H_R_U>().isCollision4HandRT = false;
            handRightUpper.GetComponent<Renderer>().material.color = Color.yellow;
            handLeftUpper.SetActive(false);
            handRightUpper.SetActive(false);

            if (isInitRehabilitation) {
                // Good effect and audio
                goodEffect.GetComponent<Emit>().IsEmit = true;
                audioSource.PlayOneShot(goodAudio);

            } else {
                
                if (isClear4H_LR_D_L && isClear4H_LR_M && isClear4H_LR_D_U) {
                    isClear4HAND = true;

                    // Add score
                    if (!isInitRehabilitation) currentScore += 1;

                    // Good effect and audio
                    goodEffect.GetComponent<Emit>().IsEmit = true;
                    audioSource.PlayOneShot(goodAudio);

                } else {
                    // Display advice text
                    //if (!isActive4Advice) {
                        DisplayText(adviceText, "Please keep your forearms parallel.");
                        adviceLabel.SetActive(true);
                        isActive4Advice = true;

                        Invoke("DisactivateAdviceText", 5f);
                    //} // end if
                } // end if

            } // end if

            // To the next step
            if (isInitRehabilitation) isInitRehabilitation = false;
            isInit = true;
            isStep_Raise_Hands = false;
            isStep_Lower_Hands = true;
        } // end if
    } // end DetectCollision4HAND_LR_UPPER()

    private void DetectCollision4HAND_LR_DIAG_UPPER() {
        bool isCollision4H_L_D_U = handLeftDiagUpper.GetComponent<DetectCollision4H_L_D_U>().isCollision4HandLT;
        bool isCollision4H_R_D_U = handRightDiagUpper.GetComponent<DetectCollision4H_R_D_U>().isCollision4HandRT;
        if (!isClear4H_LR_D_U && isCollision4H_L_D_U && isCollision4H_R_D_U) {
            isClear4H_LR_D_U = true;

            // Apply collisions
            handLeftDiagUpper.GetComponent<DetectCollision4H_L_D_U>().isCollision4HandLT = false;
            handLeftDiagUpper.GetComponent<Renderer>().material.color = Color.yellow;
            handRightDiagUpper.GetComponent<DetectCollision4H_R_D_U>().isCollision4HandRT = false;
            handRightDiagUpper.GetComponent<Renderer>().material.color = Color.yellow;
            handLeftDiagUpper.SetActive(false);
            handRightDiagUpper.SetActive(false);

            // Audio
            if (isStep_Lower_Hands) {
                audioSource.PlayOneShot(doAudio);
            } else if (isStep_Raise_Hands) {
                audioSource.PlayOneShot(miAudio);
            }

        } // end if
    } // end DetectCollision4HAND_LR_DIAG_UPPER()

    private void DetectCollision4HAND_LR_MIDDLE() {
        bool isCollision4H_L_M = handLeftMiddle.GetComponent<DetectCollision4H_L_M>().isCollision4HandLT;
        bool isCollision4H_R_M = handRightMiddle.GetComponent<DetectCollision4H_R_M>().isCollision4HandRT;
        if (!isClear4H_LR_M && isCollision4H_L_M && isCollision4H_R_M) {
            isClear4H_LR_M = true;

            // Apply collisions
            handLeftMiddle.GetComponent<DetectCollision4H_L_M>().isCollision4HandLT = false;
            handLeftMiddle.GetComponent<Renderer>().material.color = Color.yellow;
            handRightMiddle.GetComponent<DetectCollision4H_R_M>().isCollision4HandRT = false;
            handRightMiddle.GetComponent<Renderer>().material.color = Color.yellow;
            handLeftMiddle.SetActive(false);
            handRightMiddle.SetActive(false);

            // Audio
            audioSource.PlayOneShot(reAudio);

        } // end if
    } // end DetectCollision4HAND_LR_MIDDLE()
    
    private void DetectCollision4HAND_LR_DIAG_LOWER() {
        bool isCollision4H_L_D_L = handLeftDiagLower.GetComponent<DetectCollision4H_L_D_L>().isCollision4HandLT;
        bool isCollision4H_R_D_L = handRightDiagLower.GetComponent<DetectCollision4H_R_D_L>().isCollision4HandRT;
        if (!isClear4H_LR_D_L && isCollision4H_L_D_L && isCollision4H_R_D_L) {
            isClear4H_LR_D_L = true;

            // Apply collisions
            handLeftDiagLower.GetComponent<DetectCollision4H_L_D_L>().isCollision4HandLT = false;
            handLeftDiagLower.GetComponent<Renderer>().material.color = Color.yellow;
            handRightDiagLower.GetComponent<DetectCollision4H_R_D_L>().isCollision4HandRT = false;
            handRightDiagLower.GetComponent<Renderer>().material.color = Color.yellow;
            handLeftDiagLower.SetActive(false);
            handRightDiagLower.SetActive(false);

            // Audio
            if (isStep_Lower_Hands) {
                audioSource.PlayOneShot(miAudio);
            } else if (isStep_Raise_Hands) {
                audioSource.PlayOneShot(doAudio);
            }

        } // end if
    } // end DetectCollision4HAND_LR_DIAG_LOWER()

    private void DetectCollision4HAND_LR_LOWER() {
        bool isCollision4H_L_L = handLeftLower.GetComponent<DetectCollision4H_L_L>().isCollision4HandLT;
        bool isCollision4H_R_L = handRightLower.GetComponent<DetectCollision4H_R_L>().isCollision4HandRT;
        if (isCollision4H_L_L && isCollision4H_R_L) {
            // Count the number of times
            remainingTimes -= 1;

            // Apply collisions
            handLeftLower.GetComponent<DetectCollision4H_L_L>().isCollision4HandLT = false;
            handLeftLower.GetComponent<Renderer>().material.color = Color.yellow;
            handRightLower.GetComponent<DetectCollision4H_R_L>().isCollision4HandRT = false;
            handRightLower.GetComponent<Renderer>().material.color = Color.yellow;
            handLeftLower.SetActive(false);
            handRightLower.SetActive(false);

            if (isClear4H_LR_D_U && isClear4H_LR_M && isClear4H_LR_D_L) {
                isClear4HAND = true;

                // Add score
                if (!isInitRehabilitation) currentScore += 1;

                // Good effect and audio
                goodEffect.GetComponent<Emit>().IsEmit = true;
                audioSource.PlayOneShot(goodAudio);

            } else {
                // Bad effect and audio
                badEffect.GetComponent<Emit>().IsEmit = true;
                audioSource.PlayOneShot(badAudio);

                // Display advice text
                //if (!isActive4Advice) {
                    DisplayText(adviceText, "Please keep your forearms parallel.");
                    adviceLabel.SetActive(true);
                    isActive4Advice = true;

                    Invoke("DisactivateAdviceText", 5f);
                //} // end if
            } // end if

            // To the next step
            isInit = true;
            isStep_Lower_Hands = false;
            isStep_Raise_Hands = true;
        } // end if
    } // end DetectCollision4HAND_LR_LOWER()

    private void ResetIsClear() {
        //isClear4H_LR_U    = false;
        isClear4H_LR_D_U    = false;
        isClear4H_LR_M      = false;
        isClear4H_LR_D_L    = false;
        //isClear4H_LR_L    = false;

        isClear4HAND        = false;
    }

    private void DisactivateAdviceText() {
        isActive4Advice = false;
        adviceLabel.SetActive(false);
    }

    private void DisplayText(GameObject _go,  string _text) {
        _go.GetComponent<Text>().text = _text;
    }

    private void LoadResultScene() {
        userModel.SetActive(false);
        gestureListener.SetActive(false);
        kinectController.SetActive(false);
        SceneManager.LoadScene("Result");
    }

    public static float GetCurrentScore() {
        return (float)currentScore;
    }

    public static float GetAccuracyRate() {
        return (float)currentScore / (float)maxTimes;
    }
    
} // end class
