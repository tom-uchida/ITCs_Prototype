using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollision4E_L : MonoBehaviour
{
    public bool isCollision4ElbowLT=false;

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

        // Check if the collision target is an elbow
        if ( other.gameObject.name == "joint_ElbowLT" ) {
            this.isCollision4ElbowLT = true;
        }
    }
 
    private void OnTriggerExit(Collider other)
    {
        GetComponent<Renderer>().material.color = Color.white;

        this.isCollision4ElbowLT = false;
    }
}
