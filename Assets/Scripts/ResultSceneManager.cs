using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultSceneManager : MonoBehaviour
{
    // Exp
    [SerializeField] Slider expSlider;
    [SerializeField, Range(0.0f, 1.0f)] float expPercent;

    [SerializeField] Text expCurValueText, expMaxValueText; 
    [SerializeField] float expMaxValue; 

    // Other parames
    [SerializeField] CircleSlider[] circleSlider;
    [SerializeField] [Range(0.0f, 1.0f)] float[] percent;

    private bool isInit;

    void Awake() {
        
    }
    
    void Start()
    {
        isInit = true;   
    }

    void Update()
    {
        if (isInit) {
            // Get score
             float expCurValue = (float)Facilitator.getCurrentScore();

            // Exp
            expPercent = expCurValue / expMaxValue;
            expSlider.value = expPercent;
            expCurValueText.text = (expMaxValue * expPercent).ToString();
            expMaxValueText.text = (expMaxValue).ToString();

            // Circle
            // for (int i = 0; i < circleSlider.Length; i++) {
            //     circleSlider[i].Rate = percent[i];
            // }
            circleSlider[0].Rate = Facilitator.getAccuracyRate();
            circleSlider[1].Rate = 1.0f;
            circleSlider[2].Rate = 0.0f;

            isInit = false;
        } // end if
    }

	public void ToTitleScene() {
		SceneManager.LoadScene ("Title");
	}
}
