using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollision4H_L_U : MonoBehaviour
{
    public bool isCollision4HandLT = false;

    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<Renderer>().material.color = Color.white;
        GetComponent<Renderer>().material.color = Color.yellow;
    }

    // Update is called once per frame
    void Update()
    {

    }
 
    private void OnTriggerEnter(Collider other)
    {
    
    }
 
    private void OnTriggerStay(Collider other)
    {
        GetComponent<Renderer>().material.color = Color.red;

        // Check if the collision target is an hand
        if ( other.gameObject.name == "joint_HandLT" ) {
            this.isCollision4HandLT = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GetComponent<Renderer>().material.color = Color.yellow;
        this.isCollision4HandLT = false;

        // Order is important
        //this.gameObject.SetActive(false);
    }
}
