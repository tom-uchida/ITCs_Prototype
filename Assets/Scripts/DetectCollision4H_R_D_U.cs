using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollision4H_R_D_U : MonoBehaviour
{
    public bool isCollision4HandRT = false;
    public bool isCollisionEnter4HandRT = false;

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
        if ( other.gameObject.name == "joint_HandRT" ) {
            isCollisionEnter4HandRT = true;
        }
    }
 
    private void OnTriggerStay(Collider other)
    {
        GetComponent<Renderer>().material.color = Color.red;

        // Check if the collision target is an hand
        if ( other.gameObject.name == "joint_HandRT" ) {
            isCollision4HandRT = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GetComponent<Renderer>().material.color = Color.yellow;
        isCollision4HandRT = false;

        // Order is important
        gameObject.SetActive(false);
    }
}
