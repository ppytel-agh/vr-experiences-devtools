using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class XROriginStartPositions : MonoBehaviour
{
    public Transform xrOriginInstance;
    public Transform independentStartPosition;
    public List<Transform> portalExitPosition;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(xrOriginInstance);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void moveXROriginToStartPosition(Transform startPosition)
    {
        xrOriginInstance.position = startPosition.position;
        xrOriginInstance.rotation = startPosition.rotation;
    }

    [ContextMenu("move to start")]
    public void moveXROriginToIndenepdentStart()
    {
        this.moveXROriginToStartPosition(this.independentStartPosition);
    }

    public void moveXROriginToPortalExit(int exitIndex)
    {
        this.moveXROriginToStartPosition(this.portalExitPosition[exitIndex]);
    }
}
