using Codice.Client.BaseCommands.BranchExplorer;
using System.Collections;
using System.Collections.Generic;
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
    private IntersceneTeleportationProvider teleportationProvider;

    /// <inheritdoc />
    protected override void Awake()
    {
        base.Awake();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("No player in scene");
        }

        IntersceneTeleportationProvider playerTeleporataionProvider = player.GetComponentInChildren<IntersceneTeleportationProvider>();
        if (playerTeleporataionProvider == null)
        {
            Debug.LogError("Player does not have teleporation provider");
        }

        this.teleportationProvider = playerTeleporataionProvider;
    }

    /// <inheritdoc />
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        this.teleportationProvider.sendTeleportRequest(this.destinationSceneName, this.destinationPositionName);

        base.OnSelectEntered(args);
    }
}