using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.XR.Interaction.Toolkit;

public struct RailMovementNode
{
    public Vector3 position;
    public Quaternion rotation;
    public float speedChange;
}

public struct RailMovementSegment
{
    public RailMovementNode start;
    public RailMovementNode end;
    private float GetLength()
    {
        return Vector3.Distance(this.start.position, this.end.position);
    }

    private float GetDistanceFromStart(Vector3 position)
    {
        return Vector3.Distance(this.start.position, position);
    }

    public float GetDistanceRelativeToLength(float distance)
    {
        return distance / this.GetLength();
    }

    public float GetDistanceFromStartRelativeToLength(Vector3 position)
    {
        float distanceFromStart = this.GetDistanceFromStart(position);
        float relativeDistance = this.GetDistanceRelativeToLength(distanceFromStart);
        return relativeDistance;
    }

    public Vector3 GetInterpolatedPosition(float t)
    {
        return Vector3.Lerp(this.start.position, this.end.position, t);
    }

    public Quaternion GetInterpolatedRotation(float t)
    {
        return Quaternion.Lerp(this.start.rotation, this.end.rotation, t);
    }
}

public struct RailMovementRequest
{
    public Vector3 startPosition;
    public Quaternion startRotation;
    public Vector3 endPosition;
    public Quaternion endRotation;
}

public class RailLocomotionProvider : LocomotionProvider
{
    public float initialSpeed = 1.0f;
    public float m_speed = 1.0f;

    private RailMovementRequest m_currentMovement;
    private float m_currentRailLength;

    private List<RailMovementNode> movementNodes;
    private int startNodeIndex;
    private RailMovementSegment currentSegment;

    //minimum number of nodes - 2
    public void requestNewMovementSequence(List<RailMovementNode> nodes)
    {
        this.movementNodes = nodes;
        this.startNodeIndex = 0;

        this.locomotionPhase = LocomotionPhase.Started;
    }

    private RailMovementSegment GetCurrentSegmentFromNodes()
    {
        RailMovementSegment segment;
        segment.start = this.movementNodes[this.startNodeIndex];
        segment.end = this.movementNodes[this.startNodeIndex + 1];
        return segment;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.locomotionPhase = LocomotionPhase.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        if(this.locomotionPhase == LocomotionPhase.Started)
        {
            BeginLocomotion();

            this.currentSegment = this.GetCurrentSegmentFromNodes();

            system.xrOrigin.Origin.transform.position = this.movementNodes[0].position;
            system.xrOrigin.Origin.transform.rotation = this.movementNodes[0].rotation;

            this.m_speed = this.initialSpeed;

            this.locomotionPhase = LocomotionPhase.Moving;
        }
        else if(this.locomotionPhase == LocomotionPhase.Moving)
        {
            Vector3 rigPosition = system.xrOrigin.Origin.transform.position;

            float t = this.currentSegment.GetDistanceFromStartRelativeToLength(rigPosition);
            float deltaDistance = Time.deltaTime * m_speed;
            float deltaT = this.currentSegment.GetDistanceRelativeToLength(deltaDistance);
            float tNew = t + deltaT;

            Vector3 newPosition = this.currentSegment.GetInterpolatedPosition(tNew);
            Quaternion newRotation = this.currentSegment.GetInterpolatedRotation(tNew);

            system.xrOrigin.Origin.transform.position = newPosition;
            system.xrOrigin.Origin.transform.rotation = newRotation;

            bool segmentMovementEnded = (tNew >= 1.0f);
            if (segmentMovementEnded)
            {
                bool lastSegmentEnded = (this.startNodeIndex == this.movementNodes.Count - 2);
                if(lastSegmentEnded)
                {
                    this.locomotionPhase = LocomotionPhase.Done;
                }
                else
                {
                    this.startNodeIndex++;
                    this.currentSegment = this.GetCurrentSegmentFromNodes();

                    this.m_speed += this.movementNodes[this.startNodeIndex].speedChange;
                }                
            }
        }else if(this.locomotionPhase == LocomotionPhase.Done)
        {
            EndLocomotion();

            this.locomotionPhase = LocomotionPhase.Idle;
        }
    }
}
