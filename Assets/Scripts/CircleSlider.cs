using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleSlider : MonoBehaviour
{
    [SerializeField] [Range(0.0f, 1.0f)] float rate;
    [SerializeField] Image slider;
    [SerializeField] Text percent;

    #region accessor
    public float Rate{
        set{this.rate = value;}
        get{return this.rate;}
        
    }
    #endregion
    void Start()
    {
        
    }

    void Update()
    {
        slider.fillAmount = rate;
        percent.text = (rate*100.0f).ToString() + "%";
    }
}
