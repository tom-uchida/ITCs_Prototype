using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ResultSceneSample : MonoBehaviour
{

    //exp
    [SerializeField] Slider expSlider;
    [SerializeField, Range(0.0f, 1.0f)] float expPercent;

    [SerializeField] Text expCurValueText, expMaxValueText; 
    [SerializeField] float expMaxValue; 
    float expCurValue;

    //other parames
    [SerializeField] CircleSlider[] circleSlider;
    [SerializeField] [Range(0.0f, 1.0f)] float[] percent;

    
    void Start()
    {
        
    }

    void Update()
    {
        for(int i = 0; i < circleSlider.Length; i++){
            circleSlider[i].Rate = percent[i];
        }
        expSlider.value = expPercent;
        expCurValueText.text = (expMaxValue * expPercent).ToString();
        expMaxValueText.text = (expMaxValue).ToString();

    }
}
