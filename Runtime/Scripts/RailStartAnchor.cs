using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RailStartAnchor : XRBaseInteractable
{
    public RailLocomotionProvider locomotionProvider;
    public Transform railStart;
    public Transform railEnd;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void OnSelectEntered(SelectEnterEventArgs interactor)
    {
        Debug.Log("sending rail movement request");
        RailMovementRequest request = new RailMovementRequest();
        request.startPosition = railStart.position;
        request.startRotation = railStart.rotation;
        request.endPosition = railEnd.position;
        request.endRotation = railEnd.rotation;

        //locomotionProvider.requestNewMovement(request);

        base.OnSelectEntered(interactor);
    }
}
