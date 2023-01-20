using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[SelectionBaseAttribute]
public class Train : MonoBehaviour
{
    public Transform trainTransform;
    public EntireTrain parentTrain;
    public Track currentTrack;
    public float trainLength;
    public List<Train> touchingTrains;

    public int trainID;
    public string onTrackWithTag;

    public bool collidingForwards;
    public bool collidingBackwards;

    public float directionOnImpact;

    public Vector3 direction;
    public bool wasCollidingWithBuffer;

    public void CollisionMath()
    {
        collidingForwards = false;
        collidingBackwards = false;

        foreach (Train train in touchingTrains)
        {
            float collidingAngle = Mathf.Atan2(train.trainTransform.position.y - trainTransform.position.y, train.trainTransform.position.x - trainTransform.position.x) * Mathf.Rad2Deg - trainTransform.rotation.z;
            collidingForwards = collidingAngle < 35 || collidingAngle > 325;
            collidingBackwards = collidingAngle > 125 && collidingAngle < 235;
        }
    }

    private void Update()
    {
        //Track attachment
        Vector3 trackOffset = trainTransform.position - currentTrack.trackTransform.position;
        float trackProgress = Vector3.Dot(trackOffset, currentTrack.trackTransform.right);

        if (trackProgress <= currentTrack.trackLength / -2f
            || trackProgress >= currentTrack.trackLength / 2f)
        {
            MoveToNextTrack();
        }
    }

    public void AttachToTrack(Track trackToAttachTo) //tells the train which track to follow
    {
        if (currentTrack != null)
        {
            currentTrack.trainsOnTrack--;
        }

        currentTrack = trackToAttachTo;

        currentTrack.trainsOnTrack++;
        onTrackWithTag = currentTrack.tag;

        direction = Vector3.Project(direction, currentTrack.trackTransform.right).normalized;
        float trainHeading = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        trainTransform.eulerAngles = new Vector3(0, 0, trainHeading);

        Vector3 trackOffset = trainTransform.position - currentTrack.trackTransform.position;
        trackOffset = Vector3.Project(trackOffset, currentTrack.trackTransform.right);
        trainTransform.position = currentTrack.trackTransform.position + trackOffset;
    }

    private void MoveToNextTrack() //detects which track is next
    {
        Track closestTrack = null;
        float closestDistance = 1f;

        foreach (Track track in currentTrack.touchingTracks)
        {
            float distance = (track.trackTransform.position - trainTransform.position).magnitude;
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTrack = track;
            }
        }

        if (closestTrack != null)
        {
            if (currentTrack != null && currentTrack.isPartOfSwitch && closestTrack.isPartOfSwitch && closestTrack.isApproachOfSwitch && currentTrack.parentSwitch == closestTrack.parentSwitch)
            {
                if (currentTrack.isSidingOfSwitch && !closestTrack.parentSwitch.switchDirection)
                {
                    closestTrack.parentSwitch.switchDirection = true;
                    closestTrack.parentSwitch.UpdatePoints();
                }
                else if (currentTrack.isMainlineOfSwitch && closestTrack.parentSwitch.switchDirection)
                {
                    closestTrack.parentSwitch.switchDirection = false;
                    closestTrack.parentSwitch.UpdatePoints();
                }
            }

            AttachToTrack(closestTrack);
            directionOnImpact = 0;
            wasCollidingWithBuffer = false;
        } else
        {
            if (directionOnImpact == 0)
            {
                directionOnImpact = Mathf.Sign(parentTrain.speed);
            }

            if (parentTrain.reverser == directionOnImpact)
            {
                parentTrain.throttle = 0f;
                if (parentTrain.trainUIController.trainID == trainID)
                {
                    parentTrain.trainUIController.UpdateTrainMenu(parentTrain);
                }
            }

            if (!wasCollidingWithBuffer)
            {
                parentTrain.TrainHitBuffer();
            }

            wasCollidingWithBuffer = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("Train") && collision.collider.gameObject.GetComponent<Train>().trainID != trainID)
        {
            touchingTrains.Add(collision.collider.gameObject.GetComponent<Train>());
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("Train") && collision.collider.gameObject.GetComponent<Train>().trainID != trainID)
        {
            touchingTrains.Remove(collision.collider.gameObject.GetComponent<Train>());
        }
    }

    public void setPosition()
    {
        //final position set
        trainTransform.position += direction * parentTrain.speed;
    }
}
