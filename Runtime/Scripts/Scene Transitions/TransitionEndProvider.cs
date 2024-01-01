using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.XR.Interaction.Toolkit;

public class TransitionEndProvider : LocomotionProvider
{
    public float exitDelay;

    private string exitPositionName;
    
    private bool awaitingRequest = false;
    private bool hasExclusiveLocomotion = false;
    private float locomotionStartTime;
    public void teleportationExit(string exitPositionName)
    {
        Debug.Log($"requested transition end to {exitPositionName}");
        this.exitPositionName = exitPositionName;
        this.awaitingRequest = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.awaitingRequest)
        {
            locomotionPhase = LocomotionPhase.Idle;
            return;
        }

        if (!this.hasExclusiveLocomotion)
        {
            if (!BeginLocomotion())
                return;

            this.hasExclusiveLocomotion = true;
            locomotionPhase = LocomotionPhase.Started;
            this.locomotionStartTime = Time.time;
            GameObject destinationPosition = GameObject.Find(this.exitPositionName);
            //get position and rotation
            if (destinationPosition != null)
            {
                Transform exitDestination = destinationPosition.transform;
                //move rig to position
                XROrigin origin = Object.FindObjectOfType<XROrigin>();
                origin.transform.position = exitDestination.position;
                origin.transform.rotation = exitDestination.rotation;                
            }
            else
            {
                Debug.LogError($"No exit by name{this.exitPositionName}");
            }
            locomotionPhase = LocomotionPhase.Moving;
        }


        if (this.exitDelay > 0f && Time.time - this.locomotionStartTime < this.exitDelay)
            return;

        EndLocomotion();
        this.hasExclusiveLocomotion = false;
        this.awaitingRequest = false;
        locomotionPhase = LocomotionPhase.Done;
    }
}
