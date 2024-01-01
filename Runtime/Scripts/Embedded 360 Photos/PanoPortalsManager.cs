using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PanoPortalsManager : MonoBehaviour
{
    public GameObject exit;
    List<GameObject> disabledObjects;
    Material previousSkybox;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void enterPano(Material skyboxCubemap)
    {
        //get Player root
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Transform playerRoot = player.transform;
        while (playerRoot.parent != null)
        {
            playerRoot = playerRoot.parent;
        }

        //disable every object except pano manager and xr player rig
        List<GameObject> rootObjectsToKeepEnabled = new List<GameObject>();
        rootObjectsToKeepEnabled.Add(this.gameObject);
        rootObjectsToKeepEnabled.Add(playerRoot.gameObject);              
        DisableOtherObjectsInScene(rootObjectsToKeepEnabled, out this.disabledObjects);

        //swap skyboxes while preserving previous one
        this.previousSkybox = RenderSettings.skybox;
        RenderSettings.skybox = skyboxCubemap;

        //place exit above player
        this.exit.SetActive(true);
        this.exit.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 2.5f, player.transform.position.z);
    }

    public void exitPano()
    {
        //reenable disabled objects
        foreach (GameObject obj in this.disabledObjects)
        {
            obj.SetActive(true);
        }

        //disable exit interactable
        this.exit.SetActive(false);

        //return previous skybox
        RenderSettings.skybox = this.previousSkybox;
    }

    void DisableOtherObjectsInScene(List<GameObject> rootObjectsToKeepEnabled, out List<GameObject> disabledObjects)
    {
        disabledObjects = new List<GameObject>();

        GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject obj in rootObjects)
        {
            // Check if the object should be kept enabled
            if (!rootObjectsToKeepEnabled.Contains(obj) && obj.activeSelf)
            {
                disabledObjects.Add(obj);
                obj.SetActive(false);
            }
        }
    }
}
