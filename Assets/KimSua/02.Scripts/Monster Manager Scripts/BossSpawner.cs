using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [SerializeField] private Transform bossSpawn;
    public List<GameObject> bossPrefabs = new List<GameObject>();

    void Start()
    {
        SpawnBoss();
    }

    private void SpawnBoss()
    {
        var randomIndex = Random.Range(0, bossPrefabs.Count);
        GameObject prefab = bossPrefabs[randomIndex];

        Vector3 createPos = new Vector3(6f, -3f, 0);
        Instantiate(prefab, createPos, Quaternion.identity, bossSpawn);
    }
}
