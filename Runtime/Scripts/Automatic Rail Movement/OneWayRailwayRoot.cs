using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Plastic.Newtonsoft.Json.Bson;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static PlasticGui.PlasticTableColumn;

public class OneWayRailwayRoot : XRBaseInteractable
{
    public RailNode rootNode;

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
        List<RailMovementNode> nodes = this.getNodesList();

        //assume interactor is the player and it has the locomotion provider
        GameObject interactorObject = interactor.interactorObject.transform.gameObject;
        XROrigin origin = interactorObject.GetComponentInParent<XROrigin>();
        RailLocomotionProvider locomotionProvider = origin.GetComponentInChildren<RailLocomotionProvider>();
        locomotionProvider.requestNewMovementSequence(nodes);

        base.OnSelectEntered(interactor);
    }
}
