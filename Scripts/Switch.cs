using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public bool switchDirection = false;
    public Track mainline;
    public Track siding;
    public Track approach;
    public GameObject mainlineIndicator;
    public GameObject sidingIndicator;
    public GameObject textPreventingClick;
    private Vector3 baseTextPreventingClickPosition;

    private float timeSincePreventClick = -100;

    private void Start()
    {
        UpdatePoints();
        textPreventingClick.transform.eulerAngles = new Vector3(0, 0, -gameObject.transform.rotation.z);
        baseTextPreventingClickPosition = textPreventingClick.transform.position;
    }

    private void Update()
    {
        if (Time.time - timeSincePreventClick < 3f)
        {
            textPreventingClick.gameObject.SetActive(true);
            textPreventingClick.transform.position += new Vector3(0, 0.005f, 0);
        }
        else
        {
            textPreventingClick.gameObject.SetActive(false);
        }
    }

    [ContextMenu("Toggle Switch")]
    public void TogglePoints()
    {
        if (mainline.trainsOnTrack == 0 && siding.trainsOnTrack == 0 && approach.trainsOnTrack == 0)
        {
            switchDirection = !switchDirection;
            UpdatePoints();
        } else
        {
            timeSincePreventClick = Time.time;
            textPreventingClick.transform.position = baseTextPreventingClickPosition;
        }
    }

    public void UpdatePoints()
    {
        //if switchDirection is true, siding is active
        if (switchDirection)
        {
            if (approach.touchingTracks.Contains(mainline))
            {
                approach.touchingTracks.Remove(mainline);
            }
            if (!approach.touchingTracks.Contains(siding))
            {
                approach.touchingTracks.Add(siding);
            }
        } else
        {
            if (approach.touchingTracks.Contains(siding))
            {
                approach.touchingTracks.Remove(siding);
            }
            if (!approach.touchingTracks.Contains(mainline))
            {
                approach.touchingTracks.Add(mainline);
            }
        }

        mainlineIndicator.gameObject.SetActive(!switchDirection);
        sidingIndicator.gameObject.SetActive(switchDirection);
    }
}
