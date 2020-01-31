using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Facilitator : MonoBehaviour
{
    private GameObject parentObject;
    private GameObject[] objects4Collison;
    private GameObject handLeftUpper;
    private GameObject elbowLeft;
    private GameObject handLeftLower;
    private GameObject handRightUpper;
    private GameObject elbowRight;
    private GameObject handRightLower;

    private GameObject guideText;
    private GameObject countText;
    private GameObject scoreText;
    private GameObject adviceLabel;
    private GameObject adviceText;

    private int remainingTimes;
    private int currentScore;

    public bool isFinish;
    private bool isInit;
    public bool isStep_Raise_both_Elbows_to_Shoulder_Level;
    public bool isStep_Raise_both_Hands_with_Elbows_and_Hands_are_at_Right_Angles;
    public bool isStep_Lower_both_Hands_with_Elbows_and_Hands_are_at_Right_Angles;
    private bool isActive4Advice;


    void Awake()
    {
        this.handLeftUpper   = GameObject.Find("HAND_LEFT_UPPER");
        this.elbowLeft       = GameObject.Find("ELBOW_LEFT");
        this.handLeftLower   = GameObject.Find("HAND_LEFT_LOWER");
        this.handRightUpper  = GameObject.Find("HAND_RIGHT_UPPER");
        this.elbowRight      = GameObject.Find("ELBOW_RIGHT");
        this.handRightLower  = GameObject.Find("HAND_RIGHT_LOWER");

        this.guideText       = GameObject.Find("Guide");
        this.countText       = GameObject.Find("Count");
        this.scoreText       = GameObject.Find("Score");
        this.adviceLabel     = GameObject.Find("AdviceLabel");
        this.adviceText      = GameObject.Find("AdviceText");


        // Get the parent-object
        this.parentObject    = GameObject.Find("Objects4Collision");

        // Get all child-objects
        foreach (Transform childTransform in parentObject.transform)
        {
            childTransform.gameObject.SetActive(false);
        }

        this.isFinish = false;
        this.isInit = true;
        this.isStep_Raise_both_Elbows_to_Shoulder_Level = true;   
        this.isStep_Raise_both_Hands_with_Elbows_and_Hands_are_at_Right_Angles = false;
        this.isStep_Lower_both_Hands_with_Elbows_and_Hands_are_at_Right_Angles = false;
        this.isActive4Advice = false;

        this.countText.SetActive(false);
        this.remainingTimes = 20;
        this.scoreText.SetActive(false);
        this.currentScore = 0;
        this.adviceLabel.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!this.isFinish) {

            if (this.isStep_Raise_both_Elbows_to_Shoulder_Level) {
                Raise_both_Elbows_to_Shoulder_Level();

            } else {

                if (this.isStep_Raise_both_Hands_with_Elbows_and_Hands_are_at_Right_Angles) {
                    Raise_both_Hands_with_Elbows_and_Hands_are_at_Right_Angles();

                } else if (this.isStep_Lower_both_Hands_with_Elbows_and_Hands_are_at_Right_Angles) {
                    Lower_both_Hands_with_Elbows_and_Hands_are_at_Right_Angles();
                } // end if

                // Display remaining times
                this.countText.GetComponent<Text>().text = this.remainingTimes.ToString();

                // Display current score point
                this.scoreText.GetComponent<Text>().text = "Score: " + this.currentScore + "/20";

                // To the result scene
                if (this.remainingTimes == 0) {                
                    // Display finish message
                    this.guideText.GetComponent<Text>().fontSize = 200;
                    this.guideText.GetComponent<Text>().text = "Great effort!";

                    // To the result scene
                    Invoke("LoadResultScene", 5f);
                    this.isFinish = true;
                } // end if

            } // end if

        } // end if

    } // end func

    private void Raise_both_Elbows_to_Shoulder_Level(){
        // Display guide text
        this.guideText.GetComponent<Text>().text = "Please RAISE your elbows to your shoulder level.";

        // Display the only both elbows
        if (this.isInit) {
            this.elbowLeft.SetActive(true);
            this.elbowRight.SetActive(true);

            this.isInit = false;
        } // end if
        
        // Check if both elbows are raised to shoulder level
        bool isCollision4EL = elbowLeft.GetComponent<DetectCollision4E_L>().isCollision4ElbowLT;
        bool isCollision4ER = elbowRight.GetComponent<DetectCollision4E_R>().isCollision4ElbowRT;
        if (isCollision4EL && isCollision4ER) {
            // To the next step
            this.isStep_Raise_both_Elbows_to_Shoulder_Level = false;
            this.isStep_Raise_both_Hands_with_Elbows_and_Hands_are_at_Right_Angles = true;
            this.isInit = true;

            this.countText.SetActive(true);
            this.scoreText.SetActive(true);
        } // end if
    } // end func

    private void Raise_both_Hands_with_Elbows_and_Hands_are_at_Right_Angles(){
        // Display guide text
        if (this.remainingTimes >= 18)
            this.guideText.GetComponent<Text>().text = "Keep where your elbows are, and RAISE your forearms.";
        else
            this.guideText.GetComponent<Text>().text = "RAISE";

        // Display the both hand_upper
        if (this.isInit) {
            this.handLeftUpper.SetActive(true);
            this.handRightUpper.SetActive(true);

            this.isInit = false;
        } // end if

        // Check if both hands are raised
        bool isCollision4HLU = handLeftUpper.GetComponent<DetectCollision4W_L_U>().isCollision4HandLT;
        bool isCollision4HRU = handRightUpper.GetComponent<DetectCollision4W_R_U>().isCollision4HandRT;
        if (isCollision4HLU && isCollision4HRU) {
            // Count the number of times
            this.remainingTimes -= 1;

            // Handle collisions
            handLeftUpper.GetComponent<DetectCollision4W_L_U>().isCollision4HandLT = false;
            handLeftUpper.GetComponent<Renderer>().material.color = Color.white;
            handRightUpper.GetComponent<DetectCollision4W_R_U>().isCollision4HandRT = false;
            handRightUpper.GetComponent<Renderer>().material.color = Color.white;
            this.handLeftUpper.SetActive(false);
            this.handRightUpper.SetActive(false);
            
            // Add score point only if both elbows are raised to shoulder level
            bool isCollision4EL = elbowLeft.GetComponent<DetectCollision4E_L>().isCollision4ElbowLT;
            bool isCollision4ER = elbowRight.GetComponent<DetectCollision4E_R>().isCollision4ElbowRT;
            if (isCollision4EL && isCollision4ER) {
                this.currentScore += 1;

            } else {

                // Display advice text
                if (!this.isActive4Advice) {
                    this.adviceText.GetComponent<Text>().text = "Advice: Keep your elbows on your shoulder level.";
                    this.adviceLabel.SetActive(true);
                    this.isActive4Advice = true;

                    Invoke("DisactivateAdviceText", 5f);
                }

            } // end if

            // To the next step
            this.isStep_Raise_both_Hands_with_Elbows_and_Hands_are_at_Right_Angles = false;
            this.isStep_Lower_both_Hands_with_Elbows_and_Hands_are_at_Right_Angles = true;
            this.isInit = true;
        } // end if
    } // end func

    private void Lower_both_Hands_with_Elbows_and_Hands_are_at_Right_Angles(){
        // Display guide text
        if (this.remainingTimes >= 17)
            this.guideText.GetComponent<Text>().text = "Keep where your elbows are, and LOWER your forearms.";
        else
            this.guideText.GetComponent<Text>().text = "LOWER";

        // Display the both hand_upper
        if (this.isInit) {
            this.handLeftLower.SetActive(true);
            this.handRightLower.SetActive(true);

            this.isInit = false;
        } // end if

        // Check if both hands are lowered
        bool isCollision4HLL = handLeftLower.GetComponent<DetectCollision4W_L_L>().isCollision4HandLT;
        bool isCollision4HRL = handRightLower.GetComponent<DetectCollision4W_R_L>().isCollision4HandRT;
        if (isCollision4HLL && isCollision4HRL) {
            // Count the number of times
            this.remainingTimes -= 1;

            // Handle collisions
            handLeftLower.GetComponent<DetectCollision4W_L_L>().isCollision4HandLT = false;
            handLeftLower.GetComponent<Renderer>().material.color = Color.white;
            handRightLower.GetComponent<DetectCollision4W_R_L>().isCollision4HandRT = false;
            handRightLower.GetComponent<Renderer>().material.color = Color.white;
            this.handLeftLower.SetActive(false);
            this.handRightLower.SetActive(false);
            
            // Add score point only if both elbows are raised to shoulder level
            bool isCollision4EL = elbowLeft.GetComponent<DetectCollision4E_L>().isCollision4ElbowLT;
            bool isCollision4ER = elbowRight.GetComponent<DetectCollision4E_R>().isCollision4ElbowRT;
            if (isCollision4EL && isCollision4ER) {
                this.currentScore += 1;

            } else {

                // Display advice text
                if (!this.isActive4Advice) {
                    this.adviceText.GetComponent<Text>().text = "Advice: Keep your elbows on your shoulder level.";
                    this.adviceLabel.SetActive(true);
                    this.isActive4Advice = true;

                    Invoke("DisactivateAdviceText", 5f);
                }

            } // end if

            // To the next step
            this.isStep_Lower_both_Hands_with_Elbows_and_Hands_are_at_Right_Angles = false;
            this.isStep_Raise_both_Hands_with_Elbows_and_Hands_are_at_Right_Angles = true;
            this.isInit = true;
        } // end if
    } // end func

    private void DisactivateAdviceText() {
        this.adviceLabel.SetActive(false);
        this.isActive4Advice = false;
    } // end func

    private void LoadResultScene() {
		SceneManager.LoadScene("Result", LoadSceneMode.Single);
	} // end func
} // end class
