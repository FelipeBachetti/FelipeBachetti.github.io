using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Task : MonoBehaviour
{
    public float timeLeft;
    public bool timerOn = false;
    public bool taskStatus = false;    
    public TMP_Text TimerTxt;
    public string task;

    // Start is called before the first frame update
    void Start()
    {
        timerOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerOn)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                updateTimer(timeLeft);
            }
            else
            {
                Debug.Log("die");
                timeLeft = 0;
                TimerStatus(false);
            }
        }

    }

    void updateTimer(float currentTime)
    {
        currentTime += 1;

        float minutes = Mathf.Floor(currentTime / 60);
        float seconds = Mathf.Floor(currentTime % 60);

        TimerTxt.text = task + string.Format("{0:00} : {1:00}", minutes, seconds);
    }

    public void TimerStatus(bool timerStatus)
    {
        timerOn = timerStatus;
    }
}
