using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ProjectManager : MonoBehaviour
{
    public static float elapsed_time=0.0f;

    void Awake(){
       
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        elapsed_time += Time.fixedDeltaTime;
    }
}
