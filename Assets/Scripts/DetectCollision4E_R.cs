using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollision4E_R : MonoBehaviour
{
    public bool isCollision4ElbowRT=false;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material.color = Color.white;
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
        if ( other.gameObject.name == "joint_ElbowRT" ) {
            this.isCollision4ElbowRT = true;
        }
    }
 
    private void OnTriggerExit(Collider other)
    {
        GetComponent<Renderer>().material.color = Color.white;
        this.isCollision4ElbowRT = false;
    }
}
