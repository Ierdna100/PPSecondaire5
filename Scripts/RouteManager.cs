using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RouteManager : MonoBehaviour
{
    public List<EntireTrain> listOfTrains;
    public List<int> startTimes;
    public List<string> trainDestinationTracks;
    public List<string> destinationTrackTags;
    public TrainUIController trainUIController;
    public GameController gameController;
    public GameUI gameUI;
    public bool endedGame;

    public int trainsAtDestination;

    void Start()
    {
        List<Track> allTracksInLevel = FindObjectsOfType<Track>().ToList();

        foreach (Track track in allTracksInLevel)
        {
            foreach (string tag in destinationTrackTags)
            {
                if (track.CompareTag(tag))
                {
                    destinationTrackTags.Add(tag);
                    break;
                }
            }
        }

        listOfTrains = FindObjectsOfType<EntireTrain>().ToList();

        int trainID = 0;

        //sets all train data
        foreach (EntireTrain train in listOfTrains)
        {
            train.UpdateTrainID(trainID);
            train.trainUIController = trainUIController;
            train.gameObject.SetActive(false);
            trainID++;
        }

        //set menus
        gameUI.Setup(listOfTrains.Count);
    }

    void Update()
    {
        bool allTrainsStopped = true;

        foreach (EntireTrain train in listOfTrains)
        {
            //activates trains
            if (train.spawnTime < Time.timeSinceLevelLoad)
            {
                foreach (Train carriage in train.carriagesInTrain)
                {
                    if (carriage.currentTrack.trainsOnTrack != 0)
                        break;
                }
                train.gameObject.SetActive(true);
            }
            else continue;

            //checks if at destination
            if (train.atDestination && !train.previouslyAtStation)
            {
                trainsAtDestination++;
                gameUI.UpdateTrainsArrived(trainsAtDestination);
                gameUI.AddTrainArrivedScore();
                train.previouslyAtStation = true;
            } 
            else if (!train.atDestination && train.previouslyAtStation)
            {
                trainsAtDestination--;
                train.previouslyAtStation = false;
                gameUI.UpdateTrainsArrived(trainsAtDestination);
                gameUI.AddScore(-2500);
            }

            //checks if stopped
            allTrainsStopped = allTrainsStopped & train.speed == 0;
        }

        if (trainsAtDestination == listOfTrains.Count && allTrainsStopped && !endedGame)
        {
            gameController.EndGame(Time.timeSinceLevelLoad);
            endedGame = true;
        }
    }
}
