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
    private bool isStep_Raise_both_Arms_with_Elbows_and_Arms_are_at_Right_Angles;

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
        this.isStep_Raise_both_Arms_with_Elbows_and_Arms_are_at_Right_Angles=false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (this.isStep_Raise_both_Elbows_to_Shoulder_Level) {
            Raise_both_Elbows_to_Shoulder_Level();
        }

        if (this.isStep_Raise_both_Arms_with_Elbows_and_Arms_are_at_Right_Angles) {
            Raise_both_Arms_with_Elbows_and_Arms_are_at_Right_Angles();
            // 両肘が肩の高さまで上がっているかは常にチェック
        }
    }

    void Raise_both_Elbows_to_Shoulder_Level(){
        // Display the only both elbows
        if (isInit) {
            this.elbowLeft.SetActive(true);
            this.elbowRight.SetActive(true);

            this.isInit=false;
        }
        
        // Check if both elbows are raised to shoulder level
        bool isCollision4EL = elbowLeft.GetComponent<DetectCollision4E_L>().isCollision4ElbowLT;
        bool isCollision4ER = elbowRight.GetComponent<DetectCollision4E_R>().isCollision4ElbowRT;
        if (isCollision4EL && isCollision4ER) {
            this.isStep_Raise_both_Elbows_to_Shoulder_Level=false;
            this.isStep_Raise_both_Arms_with_Elbows_and_Arms_are_at_Right_Angles=true;
            this.isInit=true;
        }
    }

    void Raise_both_Arms_with_Elbows_and_Arms_are_at_Right_Angles(){
        // Display the both wrist_upper
        if (isInit) {
            this.wristLeftUpper.SetActive(true);
            this.wristRightUpper.SetActive(true);

            this.isInit=false;
        }

        this.wristLeftLower.SetActive(true);
        this.wristRightLower.SetActive(true);
    }
}
