using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

//based on TeleportationProvider
public class IntersceneTeleportationProvider : LocomotionProvider
{
    [SerializeField]
    [Tooltip("The time (in seconds) to delay the teleportation once it is activated.")]
    float m_DelayTime;

    /// <summary>
    /// The time (in seconds) to delay the teleportation once it is activated.
    /// This delay can be used, for example, as time to set a tunneling vignette effect as a VR comfort option.
    /// </summary>
    public float delayTime
    {
        get => m_DelayTime;
        set => m_DelayTime = value;
    }

    private bool awaitingRequest = false;
    private string destinationSceneName;
    private string destinationPositionName;

    /// <summary>
    /// This function will queue a teleportation request within the provider.
    /// </summary>
    /// <param name="teleportRequest">The teleportation request to queue.</param>
    /// <returns>Returns <see langword="true"/> if successfully queued. Otherwise, returns <see langword="false"/>.</returns>
    public void sendTeleportRequest(string destinationSceneName, string destinationPositionName)
    {
        this.destinationSceneName = destinationSceneName;
        this.destinationPositionName = destinationPositionName;
        Debug.Log($"sent request: teleporting to scene {this.destinationSceneName}");
        this.awaitingRequest = true;
    }

    bool m_HasExclusiveLocomotion;
    float m_TimeStarted = -1f;
    protected virtual void Update()
    {
        if (!this.awaitingRequest)
        {
            locomotionPhase = LocomotionPhase.Idle;
            return;
        }

        if (!m_HasExclusiveLocomotion)
        {
            if (!BeginLocomotion())
                return;

            m_HasExclusiveLocomotion = true;
            locomotionPhase = LocomotionPhase.Started;
            m_TimeStarted = Time.time;
        }

        // Wait for configured Delay Time
        if (m_DelayTime > 0f && Time.time - m_TimeStarted < m_DelayTime)
            return;

        locomotionPhase = LocomotionPhase.Moving;
        Debug.Log($"start coroutine: teleporting to scene {this.destinationSceneName}");
        StartCoroutine(sceneLoadCoroutine(this.destinationSceneName, this.destinationPositionName));
    }

    IEnumerator sceneLoadCoroutine(string destinationSceneName, string destinationPositionName)
    {
        Debug.Log($"teleporting to scene {destinationSceneName}");
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
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("No player in scene");
        }

        TeleportationProvider playerTeleporataionProvider = player.GetComponentInChildren<TeleportationProvider>();
        if (playerTeleporataionProvider == null)
        {
            Debug.LogError("Player does not have teleporation provider");
        }

        //get position and rotation
        GameObject destinationPosition = GameObject.Find(this.destinationPositionName);
        if (destinationPosition != null) {
            Transform exitDestination = destinationPosition.transform;
            TeleportRequest exitSceneSpawn = new TeleportRequest();
            exitSceneSpawn.destinationPosition = new Vector3(exitDestination.position.x, exitDestination.position.y, exitDestination.position.z);
            exitSceneSpawn.destinationRotation = new Quaternion(exitDestination.rotation.x, exitDestination.rotation.y, exitDestination.rotation.z, exitDestination.rotation.w);

            playerTeleporataionProvider.QueueTeleportRequest(exitSceneSpawn);
        }
        else
        {
            Debug.LogError($"No exit by name{this.destinationPositionName}");
        }

        //EndLocomotion();
        //m_HasExclusiveLocomotion = false;
        //this.awaitingRequest = false;
        //locomotionPhase = LocomotionPhase.Done;
    }
}
