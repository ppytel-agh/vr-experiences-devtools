using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RailStartAnchor : XRBaseInteractable
{
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
        
		List<RailMovementNode> movementNodes = new List<RailMovementNode>();
        RailMovementNode startNode = new RailMovementNode
        {
            position = railStart.position,
            rotation = railStart.rotation,
            speedChange = 0.0f
        };
        RailMovementNode endNode = new RailMovementNode
        {
            position = railEnd.position,
            rotation = railEnd.rotation,
            speedChange = 0.0f
        };
        movementNodes.Add(startNode);
        movementNodes.Add(endNode);

        GameObject interactorObject = interactor.interactorObject.transform.gameObject;
        XROrigin origin = interactorObject.GetComponentInParent<XROrigin>();
        RailLocomotionProvider locomotionProvider = origin.GetComponentInChildren<RailLocomotionProvider>();
        locomotionProvider.requestNewMovementSequence(movementNodes);

        base.OnSelectEntered(interactor);
    }
}
