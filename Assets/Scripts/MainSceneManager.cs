using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainSceneManager : MonoBehaviour
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
        Invoke("ActivateFacilitator", 3f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        elapsed_time += Time.fixedDeltaTime;

        // if ( Input.GetKey(KeyCode.Space) ) {
        // }
    }

    private void ActivateFacilitator(){
        this.facilitator.SetActive(true);
    }
}
