using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class OneWayRailwayRoot : XRBaseInteractable
{
    public RailNode rootNode;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private List<RailMovementNode> getNodesList()
    {
        List<RailMovementNode> nodesList = new List<RailMovementNode>();
        RailNode currentNode = this.rootNode;

        while(currentNode != null)
        {
            RailMovementNode movementNode = currentNode.GetMovementNode();
            nodesList.Add(movementNode);
            currentNode = currentNode.nextNode;
        }

        return nodesList;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs interactor)
    {
        Debug.Log("triggered one way railway");

        RailMovementRequest request = this.rootNode.GetMovementToNextNodeRequest();

        List<RailMovementNode> nodes = this.getNodesList();

        //assume interactor is the player and it has the locomotion provider
        GameObject interactorObject = interactor.interactorObject.transform.gameObject;
        XROrigin origin = interactorObject.GetComponentInParent<XROrigin>();
        RailLocomotionProvider locomotionProvider = origin.GetComponentInChildren<RailLocomotionProvider>();
        locomotionProvider.requestNewMovementSequence(nodes);

        base.OnSelectEntered(interactor);
    }
}
