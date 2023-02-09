using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class time : MonoBehaviour
{
    bool stopwatchActive = true;
    float currentTime;
    float currentScore;
    float scoreMultiplier;
    float scorePerSecond;
    public TextMeshProUGUI currentTimeText;
    public TextMeshProUGUI score;
    public TextMeshProUGUI highScoreText;
    float highScore;
    void Start()
    {
        currentTime = 0f;
        currentScore = 0f;
        scoreMultiplier = Time.deltaTime/10;
        scorePerSecond = 1f;
        highScore = PlayerPrefs.GetFloat("HighScore");
        highScoreText.text = "Best: " + (int)highScore;
    }
    void Update()
    {
        if (Time.timeScale > 0f)
        {
            scoreMultiplier = scoreMultiplier + 0.00001f;
            score.text = (int)currentScore + "";
            currentScore += scorePerSecond * scoreMultiplier;
        }
        if ((Time.timeScale == 0) && (currentScore >= highScore))
        {
            highScoreText.text = "Best: " + (int)highScore;
            highScore = currentScore;
            PlayerPrefs.SetFloat("HighScore", currentScore);
            PlayerPrefs.Save();
        }
        if (stopwatchActive == true)
        {
            currentTime = currentTime + Time.deltaTime;
        }
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        currentTimeText.text = time.ToString(@"mm\:ss\:fff");
    }

}
