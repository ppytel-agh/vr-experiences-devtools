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
    private Transform playerRoot;
    protected override bool GenerateTeleportRequest(IXRInteractor interactor, RaycastHit raycastHit, ref TeleportRequest teleportRequest)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        this.playerRoot = player.transform;

        while (playerRoot.parent != null)
        {
            this.playerRoot = playerRoot.parent;
        }
        DontDestroyOnLoad(exitDestination.gameObject);
        DontDestroyOnLoad(this.playerRoot);

        // Scene enterScene = SceneManager.GetActiveScene();
        this.transfer();
        //SceneManager.LoadScene(this.exitSceneName, LoadSceneMode.Single);



        teleportRequest.destinationPosition = exitDestination.position;
        teleportRequest.destinationRotation = exitDestination.rotation;

       //Destroy(exitDestination.gameObject);

        return true;
    }

    private void transfer()
    {
        string previousSceneName = SceneManager.GetActiveScene().name;
        DontDestroyOnLoad(this.gameObject);
        StartCoroutine(sceneLoadCoroutine(previousSceneName, this.exitSceneName));
    }

    IEnumerator sceneLoadCoroutine(string previousSceneName, string destinationSceneName)
    {
        Debug.Log("entered scene load coroutine");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(destinationSceneName);

        asyncLoad.completed += OnSceneLoadCompleted;

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            Debug.Log("scene load async loop");
            yield return null;
        }
    }

    private void OnSceneLoadCompleted(AsyncOperation asyncOperation)
    {
        Debug.Log("post scene load");

        //remove other event systems
        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject playerInstance in allPlayers)
        {
            Debug.Log(playerInstance);
            Transform playerInstanceRoot = playerInstance.transform;

            while (playerInstanceRoot.parent != null)
            {
                playerInstanceRoot = playerInstanceRoot.parent;
            }
            if (playerInstanceRoot != this.playerRoot)
            {
                Destroy(playerInstanceRoot.gameObject);
            }
        }
        //GameObject[] portals = GameObject.FindGameObjectsWithTag("Portal");
        //foreach (GameObject portal in portals)
        //{
        //    string portalDestionation = portal.GetComponent<ScenesPortal>().destinationSceneName;
        //    if (portalDestionation == previousSceneName)
        //    {
        //        player.transform.position = portal.transform.position;
        //        player.transform.rotation = portal.transform.rotation;
        //        player.transform.Translate(Vector3.forward * 2.5f);
        //        break;
        //    }
        //}
        Debug.Log("end of scene load");
    }
}