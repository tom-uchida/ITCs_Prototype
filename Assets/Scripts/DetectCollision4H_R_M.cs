using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollision4H_R_M : MonoBehaviour
{
    public bool isCollision4HandRT=false;

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

        // Check if the collision target is an hand
        if ( other.gameObject.name == "joint_HandRT" ) {
            this.isCollision4HandRT = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GetComponent<Renderer>().material.color = Color.white;
        this.isCollision4HandRT = false;

        // Order is important
        this.gameObject.SetActive(false);
    }
}
