using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private bool isInit;
    private bool isStep_Raise_both_Elbows_to_Shoulder_Level;
    private bool isStep_Raise_both_Hands_with_Elbows_and_Hands_are_at_Right_Angles;
    private bool isStep_Lower_both_Hands_with_Elbows_and_Hands_are_at_Right_Angles;


    void Awake()
    {
        this.handLeftUpper   = GameObject.Find("HAND_LEFT_UPPER");
        this.elbowLeft       = GameObject.Find("ELBOW_LEFT");
        this.handLeftLower   = GameObject.Find("HAND_LEFT_LOWER");
        this.handRightUpper  = GameObject.Find("HAND_RIGHT_UPPER");
        this.elbowRight      = GameObject.Find("ELBOW_RIGHT");
        this.handRightLower  = GameObject.Find("HAND_RIGHT_LOWER");

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
        this.isStep_Raise_both_Hands_with_Elbows_and_Hands_are_at_Right_Angles=false;
        this.isStep_Lower_both_Hands_with_Elbows_and_Hands_are_at_Right_Angles=false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (this.isStep_Raise_both_Elbows_to_Shoulder_Level) {
            Raise_both_Elbows_to_Shoulder_Level();
        }

        // 両肘が肩の高さまで上がっているかは常にチェック

        if (this.isStep_Raise_both_Hands_with_Elbows_and_Hands_are_at_Right_Angles) {
            Raise_both_Hands_with_Elbows_and_Hands_are_at_Right_Angles();

        } else if (this.isStep_Raise_both_Hands_with_Elbows_and_Hands_are_at_Right_Angles) {
            Lower_both_Hands_with_Elbows_and_Hands_are_at_Right_Angles();
        }
    }

    void Raise_both_Elbows_to_Shoulder_Level(){
        // Display the only both elbows
        if (isInit) {
            this.elbowLeft.SetActive(true);
            this.elbowRight.SetActive(true);

            this.isInit=false;
        } // end if
        
        // Check if both elbows are raised to shoulder level
        bool isCollision4EL = elbowLeft.GetComponent<DetectCollision4E_L>().isCollision4ElbowLT;
        bool isCollision4ER = elbowRight.GetComponent<DetectCollision4E_R>().isCollision4ElbowRT;
        if (isCollision4EL && isCollision4ER) {
            // To the next step
            this.isStep_Raise_both_Elbows_to_Shoulder_Level=false;
            this.isStep_Raise_both_Hands_with_Elbows_and_Hands_are_at_Right_Angles=true;
            this.isInit=true;
        } // end if
    } // end func

    void Raise_both_Hands_with_Elbows_and_Hands_are_at_Right_Angles(){
        // Display the both hand_upper
        if (isInit) {
            this.handLeftUpper.SetActive(true);
            this.handRightUpper.SetActive(true);

            this.isInit=false;
        } // end if

        // Check if both hands are raised
        bool isCollision4HLU = handLeftUpper.GetComponent<DetectCollision4W_L_U>().isCollision4HandLT;
        bool isCollision4HRU = handRightUpper.GetComponent<DetectCollision4W_R_U>().isCollision4HandRT;
        if (isCollision4HLU && isCollision4HRU) {
            this.handLeftUpper.SetActive(false);
            this.handRightUpper.SetActive(false);
            
            // 両肘が接触していなかった場合は、加点はしない

            // To the next step
            this.isStep_Raise_both_Hands_with_Elbows_and_Hands_are_at_Right_Angles=false;
            this.isStep_Lower_both_Hands_with_Elbows_and_Hands_are_at_Right_Angles=true;
            this.isInit=true;
        } // end if
    } // end func

    void Lower_both_Hands_with_Elbows_and_Hands_are_at_Right_Angles(){
        // Display the both hand_upper
        if (isInit) {
            this.handLeftLower.SetActive(true);
            this.handRightLower.SetActive(true);

            this.isInit=false;
        } // end if

        // Check if both hands are lowered
        bool isCollision4HLL = handLeftLower.GetComponent<DetectCollision4W_L_L>().isCollision4HandLT;
        bool isCollision4HRL = handRightLower.GetComponent<DetectCollision4W_R_L>().isCollision4HandRT;
        if (isCollision4HLL && isCollision4HRL) {
            this.handLeftLower.SetActive(false);
            this.handRightLower.SetActive(false);
            
            // 両肘が接触していなかった場合は、加点はしない

            // To the next step
            this.isStep_Lower_both_Hands_with_Elbows_and_Hands_are_at_Right_Angles=false;
            this.isStep_Raise_both_Hands_with_Elbows_and_Hands_are_at_Right_Angles=true;
            
            this.isInit=true;
        } // end if
    }
}
