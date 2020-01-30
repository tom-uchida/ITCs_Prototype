using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private int remainingTimes;

    private GameObject scoreLabel;
    private GameObject scoreText;

    private int currentScore;

    private bool isInit;
    public bool isStep_Raise_both_Elbows_to_Shoulder_Level;
    public bool isStep_Raise_both_Hands_with_Elbows_and_Hands_are_at_Right_Angles;
    public bool isStep_Lower_both_Hands_with_Elbows_and_Hands_are_at_Right_Angles;


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
        this.scoreLabel      = GameObject.Find("ScoreLabel");
        this.scoreText       = GameObject.Find("Score");

        // Get the parent-object
        this.parentObject    = GameObject.Find("Objects4Collision");

        // Get all child-objects
        foreach (Transform childTransform in parentObject.transform)
        {
            childTransform.gameObject.SetActive(false);
        }

        this.isInit=true;
        this.isStep_Raise_both_Elbows_to_Shoulder_Level=true;   
        this.isStep_Raise_both_Hands_with_Elbows_and_Hands_are_at_Right_Angles=false;
        this.isStep_Lower_both_Hands_with_Elbows_and_Hands_are_at_Right_Angles=false;

        this.countText.SetActive(false);
        this.remainingTimes = 20;
        this.scoreLabel.SetActive(false);
        this.currentScore = 0;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
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
            this.scoreText.GetComponent<Text>().text = this.currentScore.ToString();

        } // end if
    } // end func

    void Raise_both_Elbows_to_Shoulder_Level(){
        // Display guide text
        this.guideText.GetComponent<Text>().text = "Raise your elbows to your shoulder level and keep that condition!";

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
            this.scoreLabel.SetActive(true);
        } // end if
    } // end func

    void Raise_both_Hands_with_Elbows_and_Hands_are_at_Right_Angles(){
        // Display guide text
        this.guideText.GetComponent<Text>().text = "Raise both hands with elbows and hands are at right angles";

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
            }

            // To the next step
            this.isStep_Raise_both_Hands_with_Elbows_and_Hands_are_at_Right_Angles = false;
            this.isStep_Lower_both_Hands_with_Elbows_and_Hands_are_at_Right_Angles = true;
            this.isInit = true;
        } // end if
    } // end func

    void Lower_both_Hands_with_Elbows_and_Hands_are_at_Right_Angles(){
        // Display guide text
        this.guideText.GetComponent<Text>().text = "Lower both hands with elbows and hands are at right angles";

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
            }

            // To the next step
            this.isStep_Lower_both_Hands_with_Elbows_and_Hands_are_at_Right_Angles = false;
            this.isStep_Raise_both_Hands_with_Elbows_and_Hands_are_at_Right_Angles = true;
            this.isInit = true;
        } // end if
    } // end func
} // end class
