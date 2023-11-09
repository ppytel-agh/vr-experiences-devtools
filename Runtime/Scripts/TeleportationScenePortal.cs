using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportationScenePortal : BaseTeleportationInteractable
{
    public string exitSceneName;
    public Transform exitDestination;
    protected override bool GenerateTeleportRequest(IXRInteractor interactor, RaycastHit raycastHit, ref TeleportRequest teleportRequest)
    {
        // GameObject player = GameObject.FindGameObjectWithTag("Player");
        // Transform playerRoot = player.transform;

        // while (playerRoot.parent != null)
        // {
            // playerRoot = playerRoot.parent;
        // }
        DontDestroyOnLoad(exitDestination.gameObject);
        //DontDestroyOnLoad(playerRoot);

        // Scene enterScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(this.exitSceneName, LoadSceneMode.Single);

        //remove other event systems
        // GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");

        // foreach (GameObject playerInstance in allPlayers)
        // {
            // Debug.Log(playerInstance);
            // Transform playerInstanceRoot = playerInstance.transform;

            // while (playerInstanceRoot.parent != null)
            // {
                // playerInstanceRoot = playerInstanceRoot.parent;
            // }
            // if(playerInstanceRoot != playerRoot)
            // {
                // Destroy(playerInstanceRoot.gameObject);
            // }
        // }

        teleportRequest.destinationPosition = exitDestination.position;
        teleportRequest.destinationRotation = exitDestination.rotation;

        Destroy(exitDestination.gameObject);

        return true;
    }
}