using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollision4E_R : MonoBehaviour
{
    private int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("test");
    }
 
    // 物体がトリガーに接触しとき、１度だけ呼ばれる
    private void OnTriggerEnter(Collider collision)
    {
        
        //Debug.Log("ELBOW_LEFT");
    }
 
    // 物体がトリガーに接触している間、常に呼ばれる
    private void OnTriggerStay(Collider collision)
    {
        //Sphereの色を赤にする
        GetComponent<Renderer>().material.color = Color.red;
    }
 
    // 物体がトリガーと離れたとき、１度だけ呼ばれる
    private void OnTriggerExit(Collider collision)
    {
        GetComponent<Renderer>().material.color = Color.white;
    }
}
