using Codice.Client.BaseCommands.BranchExplorer;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Utilities;
using static UnityEngine.XR.Interaction.Toolkit.BaseTeleportationInteractable;

public class TeleportationScenePortal : XRBaseInteractable
{
    public string destinationSceneName;
    public string destinationPositionName;   

    protected override void OnSelectEntered(SelectEnterEventArgs interactor)
    {
        //assume interactor is the player and it has the locomotion provider
        GameObject interactorObject = interactor.interactorObject.transform.gameObject;
        XROrigin origin = interactorObject.GetComponentInParent<XROrigin>();
        IntersceneTeleportationProvider locomotionProvider = origin.GetComponentInChildren<IntersceneTeleportationProvider>();

        locomotionProvider.sendTeleportRequest(this.destinationSceneName, this.destinationPositionName);

        base.OnSelectEntered(interactor);
    }
}