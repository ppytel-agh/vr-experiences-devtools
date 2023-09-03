using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using UnityEngine;

[ExecuteInEditMode]
public class RailwayLineRendererSynchronization : MonoBehaviour
{
    public bool enabled = false;
    public OneWayRailwayRoot railwayRoot;
    public LineRenderer railwayLineRenderer;
    //private OneWayRailwayRoot previousRailwayState;
    //private LineRenderer previousLineRendererState;   

    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("awoken RailwayLineRendererSynchronization");
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("updating RailwayLineRendererSynchronization");
        if (enabled)
        {
            Vector3[] lineRendererPositions = new Vector3[this.railwayLineRenderer.positionCount];
            this.railwayLineRenderer.GetPositions(lineRendererPositions);


            if (lineRendererPositions.Length >= 1)
            {
                if (this.railwayRoot.rootNode == null)
                {
                    GameObject newRootObject = new GameObject($"RailRootNode");
                    newRootObject.transform.parent = this.railwayRoot.transform;
                    newRootObject.AddComponent(typeof(RailNode));
                    newRootObject.transform.position = lineRendererPositions[0];
                    this.railwayRoot.rootNode = newRootObject.GetComponent<RailNode>();
                }
            }

            RailNode currentNode = this.railwayRoot.rootNode.nextNode;
            RailNode previousNode = this.railwayRoot.rootNode;
            for (int i = 1; i < lineRendererPositions.Length; i++)
            {
                if (currentNode != null)
                {
                    currentNode.transform.position = lineRendererPositions[i];                    
                }
                else
                {
                    GameObject newNodeObject = new GameObject($"RailNode({i})");
                    newNodeObject.AddComponent(typeof(RailNode));
                    RailNode newNode = newNodeObject.GetComponent<RailNode>();

                    newNodeObject.transform.parent = this.railwayRoot.transform;

                    //need previous node to bind new node
                    previousNode.nextNode = newNode;
                }

                previousNode = currentNode;
                //reference next node in chain
                currentNode = currentNode.nextNode;
            }
            //destroy rest of nodes
            while(currentNode != null)
            {
                GameObject toDelete = currentNode.gameObject;
                currentNode = currentNode.nextNode;
                DestroyImmediate(toDelete);
            }
        }
    }
}
