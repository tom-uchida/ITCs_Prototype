using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleSlider : MonoBehaviour
{
    [SerializeField] [Range(0.0f, 1.0f)] float rate;
    [SerializeField] Image slider;
    [SerializeField] Text percent;
    private float timeElapsed;

    #region accessor
    public float Rate{
        set{this.rate = value;}
        get{return this.rate;}
        
    }
    #endregion

    void Start()
    {
        timeElapsed = 0.0f;
        percent.text = (rate*100.0f).ToString() + "%";
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;

        if (ResultSceneManager.isFinishedExp) {
            if (slider.fillAmount < rate) {
                slider.fillAmount += Time.deltaTime;
            }
        }
    }
}
