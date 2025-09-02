using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private ObjectPoolQueue pool;
    [SerializeField] private float minDistance = 3f;

    private Transform player;

    void Awake()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        player = playerObj.transform;
    }

    IEnumerator Start()
    {
        for (int i = 0; i < 5; i++)
        {
            SpawnMonster();
        }
        yield return new WaitForSeconds(0.5f);
    }

    void SpawnMonster() // 풀에서 꺼내는 역할
    {
        float randomX = Random.Range(-2f, 9f);
        Vector3 createPos = new Vector3(randomX, 0f, 0);

        if (Vector2.Distance(player.position, createPos) >= minDistance)
        {
            pool.DequeueObject(createPos, Quaternion.identity);
        }
    }
}