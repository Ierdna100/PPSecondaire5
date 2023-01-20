using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TrainUIController : MonoBehaviour
{
    public Slider speedSlider;
    public Slider brakeSlider;
    public int reverser;
    public Text speedText;
    public Text trainIDText;
    public RouteManager routeManager;

    public Button reverserReverseClicked;
    public Button reverserReverseUnclicked;
    public Button reverserNeutralClicked;
    public Button reverserNeutralUnclicked;
    public Button reverserForwardsClicked;
    public Button reverserForwardsUnclicked;

    public EntireTrain trainsToUpdate;

    public int trainID;

    public void Update()
    {
        if (trainsToUpdate != null)
        {
            float displaySpeed = Mathf.Min(Mathf.Abs(Convert.ToInt32(trainsToUpdate.speed * 10000)), 999);
            speedText.text = "VITESSE: " + displaySpeed / 10;

            if (displaySpeed % 10 == 0)
                speedText.text += ".0";
        }
    }

    public void UpdateTrainMenu(EntireTrain train)
    {
        trainID = train.trainID;
        trainsToUpdate = train;
        speedSlider.value = train.throttle;
        brakeSlider.value = train.brake;
        reverser = train.reverser;
        trainIDText.text = "Vous êtes en train de contrôler:\n" + train.trainName;

        OnUpdateReverser(reverser);
    }

    public void OnUpdateThrottle()
    {
        trainsToUpdate.throttle = speedSlider.normalizedValue;
    }
    
    public void OnUpdateBrakes()
    {
        trainsToUpdate.brake = brakeSlider.normalizedValue;
    }

    public void OnUpdateReverser(int newReverserPosition)
    {
        reverser = newReverserPosition;

        trainsToUpdate.reverser = reverser;

        if (reverser == -1)
        {
            HideAllReverserButtons();
            reverserReverseUnclicked.gameObject.SetActive(false);
            reverserReverseClicked.gameObject.SetActive(true);
        } else if (reverser == 1)
        {
            HideAllReverserButtons();
            reverserForwardsUnclicked.gameObject.SetActive(false);
            reverserForwardsClicked.gameObject.SetActive(true);
        } else
        {
            HideAllReverserButtons();
            reverserNeutralUnclicked.gameObject.SetActive(false);
            reverserNeutralClicked.gameObject.SetActive(true);
        }
    }

    private void HideAllReverserButtons()
    {
        reverserReverseClicked.gameObject.SetActive(false);
        reverserReverseUnclicked.gameObject.SetActive(true);
        reverserNeutralClicked.gameObject.SetActive(false);
        reverserNeutralUnclicked.gameObject.SetActive(true);
        reverserForwardsClicked.gameObject.SetActive(false);
        reverserForwardsUnclicked.gameObject.SetActive(true);
    }
}
