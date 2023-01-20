using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[SelectionBaseAttribute]

public class MainMenuTrain : MonoBehaviour
{
    public Track currentTrack;
    public float trainLength;
    public float speed;

    public Vector3 direction;

    private void Start()
    {
        List<Track> allTracksInLevel = FindObjectsOfType<Track>().ToList();

        foreach (Track track in allTracksInLevel)
        {
            if (track.transform.position == transform.position)
            {
                AttachToTrack(track);
                break;
            }
        }
    }

    private void FixedUpdate()
    {
        transform.position += direction * speed;
    }

    private void Update()
    {
        //Track attachment
        Vector3 trackOffset = transform.position - currentTrack.trackTransform.position;
        float trackProgress = Vector3.Dot(trackOffset, currentTrack.trackTransform.right);

        if (trackProgress <= currentTrack.trackLength / -2f
            || trackProgress >= currentTrack.trackLength / 2f)
        {
            MoveToNextTrack();
        }
    }

    public void AttachToTrack(Track trackToAttachTo) //tells the train which track to follow
    {
        currentTrack = trackToAttachTo;

        direction = Vector3.Project(direction, currentTrack.trackTransform.right).normalized;
        float trainHeading = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, trainHeading);

        Vector3 trackOffset = transform.position - currentTrack.trackTransform.position;
        trackOffset = Vector3.Project(trackOffset, currentTrack.trackTransform.right);
        transform.position = currentTrack.trackTransform.position + trackOffset;
    }

    private void MoveToNextTrack() //detects which track is next
    {
        Track closestTrack = null;
        float closestDistance = 1f;

        foreach (Track track in currentTrack.touchingTracks)
        {
            float distance = (track.trackTransform.position - transform.position).magnitude;
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTrack = track;
            }
        }

        if (closestTrack != null)
        {
            AttachToTrack(closestTrack);
        }
    }
}
