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

            //delete all existing nodes
            if(this.railwayRoot.rootNode != null)
            {
                RailNode currentNode = this.railwayRoot.rootNode;
                while(currentNode.nextNode != null)
                {
                    RailNode toDelete = currentNode.nextNode;
                    currentNode = currentNode.nextNode;
                    DestroyImmediate(toDelete.gameObject);
                }
                DestroyImmediate(this.railwayRoot.rootNode.gameObject);
            }

            if (lineRendererPositions.Length >= 1)
            {
                GameObject newRootObject = new GameObject($"RailRootNode");
                newRootObject.transform.parent = this.railwayRoot.transform;

                newRootObject.transform.position = lineRendererPositions[0];

                newRootObject.AddComponent(typeof(RailNode));
                this.railwayRoot.rootNode = newRootObject.GetComponent<RailNode>();

                RailNode currentNode = this.railwayRoot.rootNode;
                for (int i = 1; i < lineRendererPositions.Length; i++)
                {
                    GameObject newNodeObject = new GameObject($"RailNode({i})");
                    newNodeObject.transform.parent = this.railwayRoot.transform;

                    newNodeObject.transform.position = lineRendererPositions[i];

                    newNodeObject.AddComponent(typeof(RailNode));
                    RailNode newNode = newNodeObject.GetComponent<RailNode>();

                    //need previous node to bind new node
                    currentNode.nextNode = newNode;

                    currentNode = newNode;
                }
            }
        }
    }
}
