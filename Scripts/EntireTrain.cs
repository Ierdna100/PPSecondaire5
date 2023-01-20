using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntireTrain : MonoBehaviour
{
    public List<Train> carriagesInTrain;
    public string destinationTrackTag;
    public int spawnTime;
    public int trainID;
    public string trainName;
    public bool atDestination;
    public TrainUIController trainUIController;
    public GameUI gameUI;

    public float throttle = 0f;
    public float brake = 0f;
    public int reverser = 0;
    public bool collidingForwards;
    public bool collidingBackwards;
    public int numberOfCars = 1;
    public float speed;
    public bool previouslyAtStation;

    public float acceleration = 0.05f;
    private float brakingAcceleration = 0.01f;
    private float maxSpeed = 0.05f;
    private float lowSpeedThreshold = 0.0003f;

    public bool wasCollidingForwards;
    public bool wasCollidingBackwards;

    private void Awake()
    {
        carriagesInTrain = GetComponentsInChildren<Train>().ToList();
        numberOfCars = carriagesInTrain.Count();

        List<Track> allTracksInLevel = FindObjectsOfType<Track>().ToList();

        foreach (Train carriage in carriagesInTrain)
        {
            carriage.parentTrain = this;

            foreach (Track track in allTracksInLevel)
            {
                if (track.transform.position == carriage.transform.position)
                {
                    carriage.AttachToTrack(track);
                    break;
                }
            }
        }
    }

    public void TrainHitBuffer()
    {
        if (trainUIController.trainID == trainID)
        {
            trainUIController.UpdateTrainMenu(this);
        }

        speed = -speed * 0.05f;

        foreach (Train train in carriagesInTrain)
        {
            train.setPosition();
        }

        gameUI.AddScore(-2000);
    }

    private void FixedUpdate()
    {
        collidingBackwards = false;
        collidingForwards = false;

        //detect collisions
        foreach (Train train in carriagesInTrain)
        {
            train.CollisionMath();
            collidingForwards = collidingForwards || train.collidingForwards;
            collidingBackwards = collidingBackwards || train.collidingBackwards;
        }

        //speed math
        speed += (((acceleration / numberOfCars) * throttle * reverser) - (brakingAcceleration * brake * Mathf.Sign(speed))) * Time.fixedDeltaTime;

        if (Mathf.Abs(speed) < lowSpeedThreshold && brake != 0)
        {
            speed = 0;
        }
        speed = Mathf.Clamp(speed, -maxSpeed, maxSpeed);

        //stop train if colliding
        if (collidingForwards && speed > 0)
        {
            speed = 0;
        }
        else if (collidingBackwards && speed < 0)
        {
            speed = 0;
        }

        if (!wasCollidingBackwards && collidingBackwards)
            gameUI.AddScore(-2500);
        if (!wasCollidingForwards && collidingForwards)
            gameUI.AddScore(-2500);

        wasCollidingBackwards = collidingBackwards;
        wasCollidingForwards = collidingForwards;

        foreach (Train train in carriagesInTrain)
        {
            train.setPosition();
        }
    }

    private void Update()
    {
        atDestination = true;

        foreach (Train carriage in carriagesInTrain)
        {
            atDestination = atDestination & carriage.currentTrack.CompareTag(destinationTrackTag);
            if (!atDestination)
                break;
        }
    }

    public void UpdateTrainID(int trainID)
    {
        this.trainID = trainID;

        foreach (Train carriage in carriagesInTrain)
        {
            carriage.trainID = trainID;
        }
    }
}
