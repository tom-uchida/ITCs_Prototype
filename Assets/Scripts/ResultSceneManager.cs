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

    [SerializeField] Text avgTimeText = null; 

    private bool isInit;
    public static bool isFinishedExpSliderAnimation;
    private float timeElapsed;

    private AudioSource audioSource;
    public AudioClip gageupAudio;

    private bool[] isFinishedAll;

    // Exercise name
    private string exerciseCurName;
    private GameObject exerciseName;
    
    void Start()
    {
        isInit = true;
        isFinishedExpSliderAnimation = false;
        timeElapsed = 0.0f;

        // Exercise name
        exerciseName = GameObject.Find("ExerciseName");
        exerciseCurName = TitleSceneManager.GetCurrentExerciseName();

        // Avg time
        if (exerciseCurName == "Raise and Lower Exercise") {
            float tmp = (float)Facilitator.GetAvgProcessingTime() * 0.001f;
            avgTimeText.text = tmp.ToString("f1") + "(s)";
        } else if (exerciseCurName == "Raise and Lower Exercise (Elbowless version)") {
            float tmp = (float)Facilitator4RL_Elbowless.GetAvgProcessingTime() * 0.001f;
            avgTimeText.text = tmp.ToString("f1") + "(s)";
        }

        // Exp
        expSlider.value = 0.0f;
        if (exerciseCurName == "Raise and Lower Exercise") {
            TitleSceneManager.expCurValue += Facilitator.GetCurrentScore();
        } else if (exerciseCurName == "Raise and Lower Exercise (Elbowless version)") {
            TitleSceneManager.expCurValue += Facilitator4RL_Elbowless.GetCurrentScore();
        }
        expPercent = TitleSceneManager.expCurValue / expMaxValue;
        expCurValueText.text = (expMaxValue * expPercent).ToString();
        expMaxValueText.text = (expMaxValue).ToString();

        // Circle Slider
        if (exerciseCurName == "Raise and Lower Exercise") {
            circleSlider[0].Rate = Facilitator.GetAccuracyRate();
        } else if (exerciseCurName == "Raise and Lower Exercise (Elbowless version)") {
            circleSlider[0].Rate = Facilitator4RL_Elbowless.GetAccuracyRate();
        }
        circleSlider[1].Rate = 1.0f;
        circleSlider[2].Rate = 0.0f;

        // Audio
        audioSource = GetComponent<AudioSource>();
        isFinishedAll = new bool[circleSlider.Length];
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;

        // Exercise name
        exerciseName.GetComponent<Text>().text = exerciseCurName;

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
        }

        // Stop audio
        if (isFinishedAll.All( value => value == true )) audioSource.Stop();
    }

	public void ToTitleScene() {
		SceneManager.LoadScene ("Title");
	}
}
