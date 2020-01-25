using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;
    private int score;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        // スコア・ハイスコアを表示する
        scoreText.text = score.ToString ();
    }

    private void Initialize ()
    {
        score = 0;
    }

    public void AddPoint (int point)
    {
        score += point;
    }
}
