using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[SelectionBaseAttribute]
public class Track : MonoBehaviour
{
    public Transform trackTransform;
    public List<Track> touchingTracks;
    public float trackLength = 1f;
    public Transform endA, endB;
    public const float threshold = 0.01f;
    public int trainsOnTrack;

    public bool isPartOfSwitch;
    public bool isApproachOfSwitch;
    public bool isMainlineOfSwitch;
    public bool isSidingOfSwitch;
    public Switch parentSwitch;

    private void Awake()
    {
        if (isPartOfSwitch)
        {
            parentSwitch = gameObject.GetComponentInParent<Switch>();
            if (parentSwitch.mainline == this)
                isMainlineOfSwitch = true;
            else if (parentSwitch.siding == this)
                isSidingOfSwitch = true;
            else if (parentSwitch.approach == this)
                isApproachOfSwitch = true;
        }

        List<Track> allTracksInLevel = FindObjectsOfType<Track>().ToList();

        foreach (Track track in allTracksInLevel)
        {
            if (track == this) continue;
            
            if (IsTrackAttached(endA, track.endA)
                || IsTrackAttached(endA, track.endB)
                || IsTrackAttached(endB, track.endA)
                || IsTrackAttached(endB, track.endB))
            {
                touchingTracks.Add(track);
            }
        }
    }

    private bool IsTrackAttached(Transform connectionA, Transform connectionB)
    {
        return (connectionA.position - connectionB.position).magnitude <= threshold
            && Vector3.Angle(connectionA.right, -connectionB.right) <= 45;
    }
}