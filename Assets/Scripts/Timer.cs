using System.Collections;
using sxr_internal;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    float timeLeft;
    bool timerStarted = false;
    [SerializeField] GameObject timerBlock; 
    [SerializeField] TMP_Text timerText;
    // Start is called before the first frame update
    void Start()
    {
        timeLeft = 300;
        timerText.gameObject.GetComponent<Renderer> ().enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.T)) {
            timerStarted = true;
            timerText.GetComponent<Renderer> ().enabled = true;
        }

        if(timerStarted)
        {
            timeLeft -= Time.deltaTime;
            updateTimer(timeLeft);
            if(timeLeft <= 0) {
                timerStarted = false;
                timeLeft = 0;
                sxr.NextTrial(); //Set reset to 0
            }
        }
        
    }

    void updateTimer(float currentTime) {
        currentTime += 1;
        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }
}
