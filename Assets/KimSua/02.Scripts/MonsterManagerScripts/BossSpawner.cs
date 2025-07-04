using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [SerializeField] private Transform bossSpawn;
    [SerializeField] private GameObject bossPrefab;

    void Start()
    {
        Vector3 createPos = new Vector3(6f, -3f, 0);
        Instantiate(bossPrefab, createPos, Quaternion.identity, bossSpawn);
    }
}
