using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportationScenePortal : BaseTeleportationInteractable
{
    public string exitSceneName;
    public Transform exitDestination;
    protected override bool GenerateTeleportRequest(IXRInteractor interactor, RaycastHit raycastHit, ref TeleportRequest teleportRequest)
    {
        DontDestroyOnLoad(exitDestination.gameObject);
        Scene enterScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(exitSceneName, LoadSceneMode.Single);
              
        teleportRequest.destinationPosition = exitDestination.position;
        teleportRequest.destinationRotation = exitDestination.rotation;

        //Destroy(exitDestination.gameObject);

        return true;
    }
}