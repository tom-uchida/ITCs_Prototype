using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class ResultSceneManager : MonoBehaviour
{
    // Exp
    [SerializeField] Slider expSlider = null;
    [SerializeField, Range(0.0f, 1.0f)] float expPercent = 0.0f;

    [SerializeField] Text expCurValueText = null;
    [SerializeField] Text expMaxValueText = null; 
    [SerializeField] float expMaxValue = 100.0f; 

    // Other parames
    [SerializeField] CircleSlider[] circleSlider = null;
    //[SerializeField] [Range(0.0f, 1.0f)] float[] percent = null;
    //[SerializeField] [Range(0.0f, 1.0f)] float[] rate = null;

    private bool isInit;
    public static bool isFinishedExpSliderAnimation;
    private float timeElapsed;

    private AudioSource audioSource;
    public AudioClip gageupAudio;

    private bool[] isFinishedAll;
    
    void Start()
    {
        isInit = true;
        isFinishedExpSliderAnimation = false;
        timeElapsed = 0.0f;

        // Exp
        expSlider.value = 0.0f;
        TitleSceneManager.expCurValue += (int)Facilitator.getCurrentScore();
        //TitleSceneManager.expCurValue += 50;
        expPercent = TitleSceneManager.expCurValue / expMaxValue;
        expCurValueText.text = (expMaxValue * expPercent).ToString();
        expMaxValueText.text = (expMaxValue).ToString();

        // Circle Slider
        circleSlider[0].Rate = Facilitator.getAccuracyRate();
        //circleSlider[0].Rate = 0.8f;
        circleSlider[1].Rate = 1.0f;
        circleSlider[2].Rate = 0.0f;

        audioSource = GetComponent<AudioSource>();
        isFinishedAll = new bool[circleSlider.Length];
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;

        // Exp slider animation
        if (timeElapsed >= 1.0f) {
            if (isInit) audioSource.Play();
            isInit = false;

            if (expSlider.value <= expPercent)  expSlider.value += Time.deltaTime;
            else                                isFinishedExpSliderAnimation = true;
        } // end if

        // Get status
        for (int i = 0; i < circleSlider.Length; i++) {
            isFinishedAll[i] = circleSlider[i].GetIsFinishedCircleSliderAnimation();
        } // end for

        // Stop audio
        if (isFinishedAll.All( value => value == true )) audioSource.Stop();
    }

	public void ToTitleScene() {
		SceneManager.LoadScene ("Title");
	}
}
