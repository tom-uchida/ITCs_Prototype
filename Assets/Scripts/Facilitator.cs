using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Facilitator : MonoBehaviour
{
    private GameObject parentObject;
    private GameObject[] objects4Collison;
    private GameObject elbowLeft;
    private GameObject handLeftUpper;
    private GameObject handLeftDiagUpper;
    private GameObject handLeftMiddle;
    private GameObject handLeftDiagLower;
    private GameObject handLeftLower;

    private GameObject elbowRight;
    private GameObject handRightUpper;
    private GameObject handRightDiagUpper;
    private GameObject handRightMiddle;
    private GameObject handRightDiagLower;
    private GameObject handRightLower;

    private GameObject guideText;
    private GameObject countText;
    private GameObject scoreText;
    private GameObject adviceLabel;
    private GameObject adviceText;

    private int remainingTimes;
    private int currentScore;

    public bool isFinishRehabilitation;
    private bool isInit;
    private bool isStep_Raise_Elbows_to_Shoulder_Level;
    private bool isStep_Raise_Hands_with_Elbows_and_Hands_are_at_Right_Angles;
    private bool isStep_Lower_Hands_with_Elbows_and_Hands_are_at_Right_Angles;
    private bool isActive4Advice;
    private bool isInitRehabilitation;


    void Awake()
    {
        elbowLeft          = GameObject.Find("ELBOW_LEFT");
        handLeftUpper      = GameObject.Find("HAND_LEFT_UPPER");
        handLeftDiagUpper  = GameObject.Find("HAND_LEFT_DIAG_UPPER");
        handLeftMiddle     = GameObject.Find("HAND_LEFT_MIDDLE");
        handLeftDiagLower  = GameObject.Find("HAND_LEFT_DIAG_LOWER");
        handLeftLower      = GameObject.Find("HAND_LEFT_LOWER");

        elbowRight         = GameObject.Find("ELBOW_RIGHT");
        handRightUpper     = GameObject.Find("HAND_RIGHT_UPPER");
        handRightDiagUpper = GameObject.Find("HAND_RIGHT_DIAG_UPPER");
        handRightMiddle    = GameObject.Find("HAND_RIGHT_MIDDLE");
        handRightDiagLower = GameObject.Find("HAND_RIGHT_DIAG_LOWER");
        handRightLower     = GameObject.Find("HAND_RIGHT_LOWER");

        guideText       = GameObject.Find("Guide");
        countText       = GameObject.Find("Count");
        scoreText       = GameObject.Find("Score");
        adviceLabel     = GameObject.Find("AdviceLabel");
        adviceText      = GameObject.Find("AdviceText");


        // Get the parent-object
        parentObject    = GameObject.Find("Objects4Collision");

        // Get all child-objects
        foreach (Transform childTransform in parentObject.transform)
        {
            childTransform.gameObject.SetActive(false);
        }

        isFinishRehabilitation = false;
        isInit = true;
        isStep_Raise_Elbows_to_Shoulder_Level = true;   
        isStep_Raise_Hands_with_Elbows_and_Hands_are_at_Right_Angles = false;
        isStep_Lower_Hands_with_Elbows_and_Hands_are_at_Right_Angles = false;
        isActive4Advice = false;
        isInitRehabilitation = true;

        countText.SetActive(false);
        remainingTimes = 20;
        scoreText.SetActive(false);
        currentScore = 0;
        adviceLabel.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isFinishRehabilitation) {

            if (isStep_Raise_Elbows_to_Shoulder_Level) {
                Raise_Elbows_to_Shoulder_Level();

            } else {

                if (isStep_Raise_Hands_with_Elbows_and_Hands_are_at_Right_Angles) {
                    Raise_Hands_with_Elbows_and_Hands_are_at_Right_Angles();

                } else if (isStep_Lower_Hands_with_Elbows_and_Hands_are_at_Right_Angles) {
                    Lower_Hands_with_Elbows_and_Hands_are_at_Right_Angles();
                } // end if

                // Display remaining times
                countText.GetComponent<Text>().text = remainingTimes.ToString();

                // Display current score point
                scoreText.GetComponent<Text>().text = "Score: " + currentScore + "/20";

                // To the result scene
                if (remainingTimes == 0) {
                    isFinishRehabilitation = true;

                    // Display finish message
                    guideText.GetComponent<Text>().fontSize = 200;
                    guideText.GetComponent<Text>().text = "Great effort!";

                    // To the result scene
                    Invoke("LoadResultScene", 5f);
                } // end if

            } // end if

        } // end if

    } // end func

    private void Raise_Elbows_to_Shoulder_Level(){
        DisplayText(guideText, "Please RAISE your elbows to your shoulder level.");

        // Display the only elbows
        if (isInit) {
            isInit = false;
            elbowLeft.SetActive(true);
            elbowRight.SetActive(true);
        } // end if
            
        // Check if elbows are raised to shoulder level
        bool isCollision4EL = elbowLeft.GetComponent<DetectCollision4E_L>().isCollision4ElbowLT;
        bool isCollision4ER = elbowRight.GetComponent<DetectCollision4E_R>().isCollision4ElbowRT;
        if (isCollision4EL && isCollision4ER) {
            // To the next step
            isStep_Raise_Elbows_to_Shoulder_Level = false;
            isStep_Raise_Hands_with_Elbows_and_Hands_are_at_Right_Angles = true;
            isInit = true;

            countText.SetActive(true);
            scoreText.SetActive(true);
        } // end if
    } // end Raise_Elbows_to_Shoulder_Level()

    private void Raise_Hands_with_Elbows_and_Hands_are_at_Right_Angles(){
        // Display guide text
        if (remainingTimes >= 18)
            DisplayText(guideText, "Keep where your elbows are, and RAISE your forearms.");
        else
            DisplayText(guideText, "RAISE");

        if (isInitRehabilitation) {
            isInitRehabilitation = false;

            // Display only the HAND_UPPER objects
            if (isInit) {
                isInit = false;
                handLeftUpper.SetActive(true);
                handRightUpper.SetActive(true);
            } // end if

            // Check if hands are raised
            bool isCollision4HLU = handLeftUpper.GetComponent<DetectCollision4H_L_U>().isCollision4HandLT;
            bool isCollision4HRU = handRightUpper.GetComponent<DetectCollision4H_R_U>().isCollision4HandRT;
            if (isCollision4HLU && isCollision4HRU) {
                // To the next step
                isStep_Raise_Hands_with_Elbows_and_Hands_are_at_Right_Angles = false;
                isStep_Lower_Hands_with_Elbows_and_Hands_are_at_Right_Angles = true;
                isInit = true;
                
                // Apply collisions
                handLeftUpper.GetComponent<DetectCollision4H_L_U>().isCollision4HandLT = false;
                handLeftUpper.GetComponent<Renderer>().material.color = Color.white;
                handRightUpper.GetComponent<DetectCollision4H_R_U>().isCollision4HandRT = false;
                handRightUpper.GetComponent<Renderer>().material.color = Color.white;
                handLeftUpper.SetActive(false);
                handRightUpper.SetActive(false);
                
                // Add score point only if elbows are also raised to shoulder level
                bool isCollision4E_L = elbowLeft.GetComponent<DetectCollision4E_L>().isCollision4ElbowLT;
                bool isCollision4E_R = elbowRight.GetComponent<DetectCollision4E_R>().isCollision4ElbowRT;
                if (isCollision4E_L && isCollision4E_R) { // Good motion
                    // Great! no complaint.

                } else { // Bad motion
                    // Display advice text
                    if (!isActive4Advice) {
                        DisplayText(adviceText, "Keep your elbows on your shoulder level.");
                        adviceLabel.SetActive(true);
                        isActive4Advice = true;

                        Invoke("DisactivateAdviceText", 5f);
                    } // end if
                } // end if
            } // end if

        } else {
            // Display objects for collision detection
            if (isInit) {
                isInit = false;
                handLeftDiagLower.SetActive(true);
                handRightDiagLower.SetActive(true);
                handLeftMiddle.SetActive(true);
                handRightMiddle.SetActive(true);
                handLeftDiagUpper.SetActive(true);
                handRightDiagUpper.SetActive(true);
            } // end if

            // Detect collision for HAND_LR_DIAG_LOWER
            bool isCollision4H_L_D_L = handLeftDiagLower.GetComponent<DetectCollision4H_L_D_L>().isCollision4HandLT;
            bool isCollision4H_R_D_L = handRightDiagLower.GetComponent<DetectCollision4H_R_D_L>().isCollision4HandRT;
            if (!isCrear4H_LR_D_L && isCollision4H_L_D_L && isCollision4H_R_D_L) {
                isCrear4H_LR_D_L = true;

                // Apply collisions
                handLeftDiagLower.GetComponent<DetectCollision4H_L_D_L>().isCollision4HandLT = false;
                handLeftDiagLower.GetComponent<Renderer>().material.color = Color.white;
                handRightDiagLower.GetComponent<DetectCollision4H_R_D_L>().isCollision4HandRT = false;
                handRightDiagLower.GetComponent<Renderer>().material.color = Color.white;
                handLeftDiagLower.SetActive(false);
                handRightDiagLower.SetActive(false);
            }

            // Detect collision for HAND_LR_MIDDLE
            
            // Detect collision for HAND_LR_DIAG_UPPER

            // Detect collision for HAND_LR_UPPER
                // Count the number of times
                // remainingTimes -= 1;

                // currentScore += 1;

        } // end if
    } // end Raise_Hands_with_Elbows_and_Hands_are_at_Right_Angles()

    private void Lower_Hands_with_Elbows_and_Hands_are_at_Right_Angles(){
        // Display guide text
        if (remainingTimes >= 17)
            DisplayText(guideText, "Keep where your elbows are, and LOWER your forearms.");
        else
            DisplayText(guideText, "LOWER");

        // Display the HAND_UPPER objects
        if (isInit) {
            isInit = false;
            handLeftLower.SetActive(true);
            handRightLower.SetActive(true);
        } // end if

        // Check if hands are lowered
        bool isCollision4HLL = handLeftLower.GetComponent<DetectCollision4H_L_L>().isCollision4HandLT;
        bool isCollision4HRL = handRightLower.GetComponent<DetectCollision4H_R_L>().isCollision4HandRT;
        if (isCollision4HLL && isCollision4HRL) {
            // Count the number of times
            remainingTimes -= 1;

            // Apply collisions
            handLeftLower.GetComponent<DetectCollision4H_L_L>().isCollision4HandLT = false;
            handLeftLower.GetComponent<Renderer>().material.color = Color.white;
            handRightLower.GetComponent<DetectCollision4H_R_L>().isCollision4HandRT = false;
            handRightLower.GetComponent<Renderer>().material.color = Color.white;
            handLeftLower.SetActive(false);
            handRightLower.SetActive(false);
            
            // Add score point only if elbows are raised to shoulder level
            bool isCollision4EL = elbowLeft.GetComponent<DetectCollision4E_L>().isCollision4ElbowLT;
            bool isCollision4ER = elbowRight.GetComponent<DetectCollision4E_R>().isCollision4ElbowRT;
            if (isCollision4EL && isCollision4ER) {
                currentScore += 1;

            } else {

                // Display advice text
                if (!isActive4Advice) {
                    isActive4Advice = true;
                    DisplayText(adviceText, "Keep your elbows on your shoulder level.");
                    adviceLabel.SetActive(true);

                    Invoke("DisactivateAdviceText", 5f);
                }

            } // end if

            // To the next step
            isStep_Lower_Hands_with_Elbows_and_Hands_are_at_Right_Angles = false;
            isStep_Raise_Hands_with_Elbows_and_Hands_are_at_Right_Angles = true;
            isInit = true;
        } // end if
    } // end Lower_Hands_with_Elbows_and_Hands_are_at_Right_Angles()

    private void DisactivateAdviceText() {
        isActive4Advice = false;
        adviceLabel.SetActive(false);
    } // end DisactivateAdviceText()

    private void DisplayText(GameObject _go,  string _text) {
        _go.GetComponent<Text>().text = _text;
    } // end DisplayText(GameObject _go,  string _text)

    private void LoadResultScene() {
		SceneManager.LoadScene("Result", LoadSceneMode.Single);
	} // end LoadResultScene()
} // end class
