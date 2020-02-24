using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleSlider : MonoBehaviour
{
    [SerializeField] [Range(0.0f, 1.0f)] float rate = 0.0f;
    [SerializeField] Image slider = null;
    [SerializeField] Text percent = null;
    private float timeElapsed;
    private bool isFinishedCircleSliderAnimation;

    #region accessor
    public float Rate{
        set{this.rate = value;}
        get{return this.rate;}
        
    }
    #endregion

    void Start()
    {
        timeElapsed = 0.0f;
        isFinishedCircleSliderAnimation = false;
        percent.text = (rate*100.0f).ToString("f0") + "%";
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;

        if (ResultSceneManager.isFinishedExpSliderAnimation) {
            if (slider.fillAmount < rate) {
                slider.fillAmount += Time.deltaTime;
            } else {
                isFinishedCircleSliderAnimation = true;
            }
        }
    }

    public bool GetIsFinishedCircleSliderAnimation() {
        return isFinishedCircleSliderAnimation;
    }
}
