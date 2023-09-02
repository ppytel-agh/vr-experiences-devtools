using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public struct RailMovementRequest
{
    public Vector3 startPosition;
    public Quaternion startRotation;
    public Vector3 endPosition;
    public Quaternion endRotation;
}

public class RailLocomotionProvider : LocomotionProvider
{
    public float m_speed = 1.0f;

    private bool m_newMovementRequest;
    private bool m_movementProceeds;
    private RailMovementRequest m_currentMovement;
    private float m_currentRailLength;
    

    public void requestNewMovement(RailMovementRequest newMovement)
    {
        Debug.Log("received rail movement reuqest");
        m_currentRailLength = Vector3.Distance(newMovement.startPosition, newMovement.endPosition);
        m_currentMovement = newMovement;

        m_newMovementRequest = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_newMovementRequest = false;
        m_movementProceeds = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_newMovementRequest)
        {
            Debug.Log("starting rail movement");
            BeginLocomotion();

            system.xrOrigin.Origin.transform.position = m_currentMovement.startPosition;
            system.xrOrigin.Origin.transform.rotation = m_currentMovement.startRotation;

            m_newMovementRequest = false;
            m_movementProceeds = true;
        }
        else if(m_movementProceeds)
        {
            Debug.Log("continuing rail movement");
            float currentDistance = Vector3.Distance(m_currentMovement.startPosition, system.xrOrigin.Origin.transform.position);
            float t = currentDistance / m_currentRailLength;
            float deltaDistance = Time.deltaTime * m_speed;
            float deltaT = deltaDistance / m_currentRailLength;
            float tNew = t + deltaT;

            Vector3 newPosition = Vector3.Lerp(m_currentMovement.startPosition, m_currentMovement.endPosition, tNew);
            Quaternion newRotation = Quaternion.Lerp(m_currentMovement.startRotation, m_currentMovement.endRotation, tNew);

            system.xrOrigin.Origin.transform.position = newPosition;
            system.xrOrigin.Origin.transform.rotation = newRotation;

            bool movementEnded = (tNew >= 1.0f);
            if (movementEnded)
            {
                Debug.Log("finished rail movement");
                EndLocomotion();

                m_movementProceeds = false;
            }
        }
    }
}
