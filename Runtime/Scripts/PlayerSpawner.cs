using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player == null)
        {
            GameObject newPlayer = Instantiate(this.playerPrefab, this.transform);
            newPlayer.transform.parent = null;
        }
    }
}
