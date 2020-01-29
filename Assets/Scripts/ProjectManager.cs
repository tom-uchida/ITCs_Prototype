using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ProjectManager : MonoBehaviour
{
    public static float elapsed_time=0.0f;

    private GameObject facilitator;

    void Awake(){
        this.facilitator = GameObject.Find("Facilitator");
    }
    // Start is called before the first frame update
    void Start()
    {
        this.facilitator.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        elapsed_time += Time.fixedDeltaTime;

        if ( Input.GetKey(KeyCode.Space) ) {
            Invoke("ActivateFacilitator", 3f);
            
        }
    }

    private void ActivateFacilitator(){
        this.facilitator.SetActive(true);
    }
}
