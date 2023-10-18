using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/SpawnParams")]
public class SpawnParams : ScriptableObject
{
    public GameObject spawnPrefab;
    public Vector3 spawnPosition;
    public Quaternion spawnRotation;
}
