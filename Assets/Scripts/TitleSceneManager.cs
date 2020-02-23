using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour
{
    // Exp
    [SerializeField] Slider expSlider = null;
    [SerializeField, Range(0.0f, 1.0f)] float expPercent = 0.0f;

    [SerializeField] Text expCurValueText = null;
    [SerializeField] Text expMaxValueText = null; 
    [SerializeField] float expMaxValue = 100.0f;

    // Added by Kawakami 2/22
    // Number of Exercises
    [SerializeField] Slider exerciseNumSlider = null;
    [SerializeField] Text exerciseNumText     = null;
    public static int currentExerciseNum;

    private int expCumValue;
    public static int expCurValue;

    void Awake() {
        expSlider.value = 0.0f;
        expCurValue = 0;

        // Added by Kawakami
        exerciseNumSlider.value = (float)10;
        currentExerciseNum = 0;
        
        // Load score
        expCumValue = PlayerPrefs.GetInt("Exp");
        expCurValue += expCumValue;
    } 

    // Start is called before the first frame update
    void Start()
    {
        expPercent = expCurValue / expMaxValue;
        expCurValueText.text = (expMaxValue * expPercent).ToString();
        expMaxValueText.text = (expMaxValue).ToString();

        expSlider.value = expPercent;
    }

    // Update is called once per frame
    void Update()
    {
        // Reset cumulaive exp
        if (Input.GetKeyDown(KeyCode.R)){
            PlayerPrefs.DeleteKey("Exp");
        }

        // Added by Kawakami
        currentExerciseNum = (int)exerciseNumSlider.value;
        exerciseNumText.text = currentExerciseNum.ToString();
    }

    // Added by Kawakami
    public static float GetCurrentExerciseNum()
    {
        return (float)currentExerciseNum;
    }

	public void ToMainScene() {
		SceneManager.LoadScene ("Main");
	}

    public void ToRaiseLowerElbowlessScene() {
		SceneManager.LoadScene ("Raise_Lower_Elbowless");
	}

    // End
    public void EndGame() {
    #if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
	#else
		Application.Quit();
	#endif
    }
}
