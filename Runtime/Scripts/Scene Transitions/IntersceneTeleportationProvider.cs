using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

//based on TeleportationProvider
public class IntersceneTeleportationProvider : LocomotionProvider
{
    public float enterDelay;

    private string destinationSceneName;
    private string destinationPositionName;

    private bool awaitingRequest = false;
    private bool hasExclusiveLocomotion = false;
    private float locomotionStartTime;

    public void sendTeleportRequest(string destinationSceneName, string destinationPositionName)
    {
        this.destinationSceneName = destinationSceneName;
        this.destinationPositionName = destinationPositionName;
        this.awaitingRequest = true;
    }

    protected virtual void Update()
    {
        if (!this.awaitingRequest)
        {
            locomotionPhase = LocomotionPhase.Idle;
            return;
        }

        if (!this.hasExclusiveLocomotion)
        {
            if (!BeginLocomotion())
                return;

            this.hasExclusiveLocomotion = true;
            locomotionPhase = LocomotionPhase.Started;
            this.locomotionStartTime = Time.time;
        }

        if (this.enterDelay > 0f && Time.time - this.locomotionStartTime < this.enterDelay)
            return;

        locomotionPhase = LocomotionPhase.Moving;
        StartCoroutine(sceneLoadCoroutine(this.destinationSceneName, this.destinationPositionName));

        EndLocomotion();
        this.hasExclusiveLocomotion = false;
        this.awaitingRequest = false;
        locomotionPhase = LocomotionPhase.Done;
    }

    IEnumerator sceneLoadCoroutine(string destinationSceneName, string destinationPositionName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(destinationSceneName);

        asyncLoad.completed += OnSceneLoadCompleted;
        this.destinationPositionName = destinationPositionName;

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    private void OnSceneLoadCompleted(AsyncOperation asyncOperation)
    {
        Debug.Log("post scene load");

        XROrigin origin = Object.FindObjectOfType<XROrigin>();
        TransitionEndProvider transitionEndProvider = origin.GetComponentInChildren<TransitionEndProvider>();

        transitionEndProvider.teleportationExit(this.destinationPositionName);
    }
}
