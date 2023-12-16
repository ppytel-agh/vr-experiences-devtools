using Codice.Client.BaseCommands.BranchExplorer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportationScenePortal : BaseTeleportationInteractable
{
    public string exitSceneName;
    public Transform exitDestination;
    private TeleportRequest exitSceneSpawn;
    protected override bool GenerateTeleportRequest(IXRInteractor interactor, RaycastHit raycastHit, ref TeleportRequest teleportRequest)
    {
        this.exitSceneSpawn = new TeleportRequest();
        this.exitSceneSpawn.destinationPosition = new Vector3(exitDestination.position.x, exitDestination.position.y, exitDestination.position.z);
        this.exitSceneSpawn.destinationRotation = new Quaternion(exitDestination.rotation.x, exitDestination.rotation.y, exitDestination.rotation.z, exitDestination.rotation.w);

        StartCoroutine(sceneLoadCoroutine());

        return true;
    }

    IEnumerator sceneLoadCoroutine()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(this.exitSceneName);

        asyncLoad.completed += OnSceneLoadCompleted;

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    private void OnSceneLoadCompleted(AsyncOperation asyncOperation)
    {
        Debug.Log("post scene load");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) {
            Debug.LogError("No player in scene");
        }

        TeleportationProvider playerTeleporataionProvider = player.GetComponentInChildren<TeleportationProvider>();
        if(playerTeleporataionProvider == null)
        {
            Debug.LogError("Player does not have teleporation provider");
        }

        playerTeleporataionProvider.QueueTeleportRequest(this.exitSceneSpawn);
    }
}