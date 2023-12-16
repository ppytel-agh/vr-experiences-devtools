using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.XR.Interaction.Toolkit;

public class TransitionEndProvider : LocomotionProvider
{
    public float exitDelay;

    private bool awaitingRequest = false;
    private string exitPositionName;
    private bool m_HasExclusiveLocomotion;
    private float m_TimeStarted;
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

        if (!m_HasExclusiveLocomotion)
        {
            if (!BeginLocomotion())
                return;

            m_HasExclusiveLocomotion = true;
            locomotionPhase = LocomotionPhase.Started;
            m_TimeStarted = Time.time;
            //move rig to position
            //get position and rotation
            GameObject destinationPosition = GameObject.Find(this.exitPositionName);
            if (destinationPosition != null)
            {
                Transform exitDestination = destinationPosition.transform;
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    Debug.Log("moving player to position");
                    player.transform.position = exitDestination.position;
                    player.transform.rotation = exitDestination.rotation;
                }
                else
                {
                    Debug.LogError("No player in scene");
                }
            }
            else
            {
                Debug.LogError($"No exit by name{this.exitPositionName}");
            }
            locomotionPhase = LocomotionPhase.Moving;
        }

        // Wait for configured Delay Time
        if (exitDelay > 0f && Time.time - m_TimeStarted < exitDelay)
            return;

        EndLocomotion();
        m_HasExclusiveLocomotion = false;
        this.awaitingRequest = false;
        locomotionPhase = LocomotionPhase.Done;
    }
}
