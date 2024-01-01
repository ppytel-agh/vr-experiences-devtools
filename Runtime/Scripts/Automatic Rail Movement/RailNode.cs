using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RailNode : MonoBehaviour
{
    public RailNode nextNode;
    public float speedchange = 0.0f;

    //assumes nextNode is not null
    public RailMovementRequest GetMovementToNextNodeRequest()
    {
        RailMovementRequest request;

        request.startPosition = this.transform.position;
        request.startRotation = this.transform.rotation;

        request.endPosition = nextNode.transform.position;
        request.endRotation = nextNode.transform.rotation;

        return request;
    }

    public RailMovementNode GetMovementNode()
    {
        RailMovementNode node;

        node.position = this.transform.position;
        node.rotation = this.transform.rotation;

        node.speedChange = this.speedchange;

        return node;
    }

    //public List<RailMovementNode> GetForwardNodesList(List<RailMovementNode> nodesList)
    //{
    //    nodesList.Append(this.GetMovementNode());
    //    nodesList = this.nextNode!.GetForwardNodesList(nodesList);
    //    return nodesList;
    //}


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
