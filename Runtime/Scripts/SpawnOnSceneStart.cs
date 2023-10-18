using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnSceneStart : MonoBehaviour
{
    public static GameObject spawnPrefab;
    public static Vector3 spawnPosition;
    public static Quaternion spawnRotation;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void BeforeSceneLoad()
    {
        Instantiate(spawnPrefab, spawnPosition, spawnRotation);
    }
}
