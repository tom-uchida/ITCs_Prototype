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

    private int expCumValue;
    public static int expCurValue;

    void Awake() {
        expSlider.value = 0.0f;
        expCurValue = 0;
        
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
    }

	public void ToMainScene() {
		SceneManager.LoadScene ("Main");
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
