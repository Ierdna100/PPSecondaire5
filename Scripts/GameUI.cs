using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameUI : MonoBehaviour
{
    public Text trainsArrivedIndicator;
    public Text timeIndicator;
    public Text scoreIndicator;

    private int trainsTotal;
    public int score;

    private void Update()
    {
        timeIndicator.text = "Temps: ";

        if (Time.timeSinceLevelLoad < 600)
            timeIndicator.text += "0" + Mathf.Floor(Time.timeSinceLevelLoad / 60).ToString();
        else
            timeIndicator.text += Mathf.Floor(Time.timeSinceLevelLoad / 60).ToString();

        timeIndicator.text += ":";
        if (Time.timeSinceLevelLoad % 60 < 10)
            timeIndicator.text += "0" + Mathf.Floor(Time.timeSinceLevelLoad % 60).ToString();
        else
            timeIndicator.text += Mathf.Floor(Time.timeSinceLevelLoad % 60).ToString();
    }

    public void Setup(int totalTrains)
    {
        trainsTotal = totalTrains;
        trainsArrivedIndicator.text = "Trains arrivés: 0 / " + trainsTotal;
    }

    public void UpdateTrainsArrived(int trainsArrived)
    {
        trainsArrivedIndicator.text = "Trains arrivés: " + trainsArrived + " / " + trainsTotal;
    }

    public void AddTrainArrivedScore()
    {
        AddScore(Convert.ToInt32(100 * (1 + Mathf.Max((600 - Time.timeSinceLevelLoad), 0) * 0.1f)));
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreIndicator.text = "Score: " + score;
    }
}
