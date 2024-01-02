using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PanoPortalsManager : MonoBehaviour
{
    public GameObject exit;
    List<GameObject> disabledObjects;
    Material previousSkybox;
    private bool inPanoMode;

    // Start is called before the first frame update
    void Start()
    {
        this.inPanoMode = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void enterPano(Material skyboxCubemap)
    {
        if(!this.inPanoMode)
        {
            //get host root
            Transform root = this.transform;
            while (root.parent != null)
            {
                root = root.parent;
            }

            //disable every object except pano manager and xr player rig
            List<GameObject> rootObjectsToKeepEnabled = new List<GameObject>();
            rootObjectsToKeepEnabled.Add(root.gameObject);
            DisableOtherObjectsInScene(rootObjectsToKeepEnabled, out this.disabledObjects);

            //swap skyboxes while preserving previous one
            this.previousSkybox = RenderSettings.skybox;

            //place exit above player
            GameObject player = root.GetComponentInChildren<XROrigin>().gameObject;
            this.exit.SetActive(true);
            this.exit.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 2.5f, player.transform.position.z);

            this.inPanoMode = true;
        }
        RenderSettings.skybox = skyboxCubemap;
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

        this.inPanoMode = false;
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
