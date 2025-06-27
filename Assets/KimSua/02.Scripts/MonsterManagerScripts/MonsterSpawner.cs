using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public List<GameObject> monsters = new List<GameObject>();
    [SerializeField] private Transform monster;
    private Transform player;
    [SerializeField] private float minDistance = 3f;

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

    void SpawnMonster()
    {
        var randomIndex = Random.Range(0, monsters.Count);
        GameObject prefab = monsters[randomIndex];
        float randomX = Random.Range(-2f, 9f);
        float spawnY;

        if (prefab.CompareTag("Fly"))
        {

            spawnY = 2f;
        }
        else
        {
            Vector2 pos = new Vector2(randomX, 10f);
            RaycastHit2D rayHit = Physics2D.Raycast(pos, Vector2.down, 15f, LayerMask.GetMask("Ground"));

            if (rayHit.collider != null)
            {
                spawnY = rayHit.point.y;
            }
            else
            {
                spawnY = -2f;
            }
        }

        Vector3 createPos = new Vector3(randomX, spawnY, 0);

        if (Vector2.Distance(player.position, createPos) >= minDistance)
        {
            Instantiate(prefab, createPos, Quaternion.identity, monster);
            return;
        }
    }
}