using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

[Serializable]
public struct TourStop
{
    public Transform guidePlatform;
    public AudioClip description;
}

public class GuidedTour : MonoBehaviour
{
    public enum TourPhase
    {
        Idle,
        RotatingTowardsNextStop,
        MovingBetweenStops,
        RotatingInStop,
        AtStop,
        PlayingDescription
    }

    public XROrigin player;
    public Transform guide;
    public float guideMovementSpeed = 1.0f;
    public float guideRotationSpeed = 1.0f;
    public float maxDistanceToPlayDescription;
    public List<TourStop> tourStops;


    private bool beginTourSignal;
    private TourPhase phase;
    private int stopIndex;
    private Transform nextStop;
    private float descriptionPlayEndTimestamp;

    public void BeginTourSignalHigh()
    {
        this.beginTourSignal = true;
    }

    public void BeginTourSignalLow()
    {
        this.beginTourSignal = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.phase = TourPhase.Idle;
    }

    private void TransitionToNextStop()
    {
        if (this.stopIndex < this.tourStops.Count - 1)
        {
            this.nextStop = this.tourStops[this.stopIndex + 1].guidePlatform;
        }
        else
        {
            this.nextStop = this.transform;
        }
        this.phase = TourPhase.RotatingTowardsNextStop;
    }

    // Update is called once per frame
    void Update()
    {
        switch(this.phase)
        {
            case TourPhase.Idle:
                if(this.beginTourSignal)
                {
                    Debug.Log("starting tour");
                    this.stopIndex = -1;
                    this.nextStop = this.tourStops[0].guidePlatform;
                    this.phase = TourPhase.RotatingTowardsNextStop;
                }
                break;
            case TourPhase.RotatingTowardsNextStop:
                float rotationDelta = this.guideRotationSpeed * Time.deltaTime;

                GameObject transformCopy = new GameObject();
                transformCopy.transform.position = this.guide.transform.position;
                transformCopy.transform.rotation = this.guide.transform.rotation;
                transformCopy.transform.LookAt(this.nextStop);
                Quaternion destinationRotation = transformCopy.transform.rotation;
                Destroy(transformCopy);

                this.guide.transform.rotation = Quaternion.RotateTowards(this.guide.transform.rotation, destinationRotation, rotationDelta);
                if(this.guide.transform.rotation == destinationRotation)
                {
                    this.phase = TourPhase.MovingBetweenStops;
                }
                break;
            case TourPhase.MovingBetweenStops:
                float movementDelta = this.guideMovementSpeed * Time.deltaTime;
                this.guide.transform.position = Vector3.MoveTowards(this.guide.transform.position, this.nextStop.position, movementDelta);
                if (this.guide.transform.position == this.nextStop.position)
                {
                    if (this.stopIndex < this.tourStops.Count - 1)
                    {
                        //works for moving from box to first stop
                        this.stopIndex++;
                    }
                    else
                    {
                        this.stopIndex = -1;
                    }
                    this.phase = TourPhase.RotatingInStop;
                }
                break;
            case TourPhase.RotatingInStop:
                float rotationDelta2 = this.guideRotationSpeed * Time.deltaTime;
                this.guide.transform.rotation = Quaternion.RotateTowards(this.guide.transform.rotation, this.nextStop.rotation, rotationDelta2);
                if (this.guide.transform.rotation == this.nextStop.rotation)
                {
                    //branch between any stop and moving back to box
                    if (this.stopIndex != -1)
                    {
                        this.phase = TourPhase.AtStop;
                    }
                    else
                    {
                        Debug.Log("end of tour");
                        this.phase = TourPhase.Idle;
                    }
                }
                break;
            case TourPhase.AtStop:
                float distanceFromPlayer = Vector3.Distance(this.guide.transform.position, this.player.transform.position);
                Debug.Log($"Player distance - {distanceFromPlayer}");
                if (distanceFromPlayer <= this.maxDistanceToPlayDescription)
                {
                    Debug.Log($"player approached {this.stopIndex}-th stop");
                    if (this.tourStops[this.stopIndex].description != null)
                    {
                        AudioClip stopClip = this.tourStops[this.stopIndex].description;                        

                        float clipTime = stopClip.length;
                        this.descriptionPlayEndTimestamp = Time.time + clipTime;

                        AudioSource guideAudioSource = this.guide.GetComponentInChildren<AudioSource>();
                        guideAudioSource.clip = stopClip;
                        guideAudioSource.Play();

                        this.phase = TourPhase.PlayingDescription;
                    }
                    else
                    {
                        TransitionToNextStop();
                    }
                }
                break;
            case TourPhase.PlayingDescription:
                if(Time.time > this.descriptionPlayEndTimestamp)
                {
                    TransitionToNextStop();
                }
                break;
        }
    }
}
