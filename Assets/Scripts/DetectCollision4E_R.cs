using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollision4E_R : MonoBehaviour
{
    private int count = 0;
    public bool isCollision=false;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {

    }
 
    private void OnTriggerEnter(Collider collision)
    {

    }
 
    private void OnTriggerStay(Collider collision)
    {
        GetComponent<Renderer>().material.color = Color.red;
        isCollision = true;
    }
 
    private void OnTriggerExit(Collider collision)
    {
        GetComponent<Renderer>().material.color = Color.white;
        isCollision = false;
    }
}
