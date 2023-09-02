using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesUtility : MonoBehaviour
{
    public string sceneName;
    public bool additive;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("load scene by name")]
    public void loadSceneOfProvidedName()
    {
        if (this.additive)
        {
            SceneManager.LoadScene(this.sceneName, LoadSceneMode.Additive);
        }
        else
        {
            SceneManager.LoadScene(this.sceneName, LoadSceneMode.Single);
        }
    }
}
