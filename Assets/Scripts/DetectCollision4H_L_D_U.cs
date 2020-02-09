using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollision4H_L_D_U : MonoBehaviour
{
    public bool isCollision4HandLT = false;
    public bool isCollisionEnter4HandLT = false;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material.color = Color.yellow;
    }

    // Update is called once per frame
    void Update()
    {

    }
 
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collision target is an hand
        if ( other.gameObject.name == "joint_HandLT" ) {
            isCollisionEnter4HandLT = true;
        }
    }
 
    private void OnTriggerStay(Collider other)
    {
        GetComponent<Renderer>().material.color = Color.red;

        // Check if the collision target is an hand
        if ( other.gameObject.name == "joint_HandLT" ) {
            isCollision4HandLT = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GetComponent<Renderer>().material.color = Color.yellow;
        isCollision4HandLT = false;
        isCollisionEnter4HandLT = false;

        // Order is important
        this.gameObject.SetActive(false);
    }
}
