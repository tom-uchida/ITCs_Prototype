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
    [SerializeField] [Range(0.0f, 1.0f)] float[] rate;

    private bool isInit;
    public static bool isFinishedExp;
    private float timeElapsed;

    void Awake() {
        
    }
    
    void Start()
    {
        isInit = true; 
        isFinishedExp = false;
        timeElapsed = 0.0f;

        // Exp
        expSlider.value = 0.0f;
        float expCurValue = (float)Facilitator.getCurrentScore();
        //float expCurValue = 67;
        expPercent = expCurValue / expMaxValue;
        expCurValueText.text = (expMaxValue * expPercent).ToString();
        expMaxValueText.text = (expMaxValue).ToString();

        // Circle Slider
        circleSlider[0].Rate = (float)Facilitator.getAccuracyRate();
        //circleSlider[0].Rate = 0.8f;
        circleSlider[1].Rate = 1.0f;
        circleSlider[2].Rate = 0.0f;
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= 1.0f) {
            if (expSlider.value <= expPercent) {
                expSlider.value += Time.deltaTime;
            } else {
                isFinishedExp = true;
            }
        } // end if
    }

	public void ToTitleScene() {
		SceneManager.LoadScene ("Title");
	}
}
