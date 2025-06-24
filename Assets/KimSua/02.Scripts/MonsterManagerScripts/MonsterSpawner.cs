using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> monsters = new List<GameObject>();
    [SerializeField] private Transform monster;
    [SerializeField] private Transform player;
    [SerializeField] private float minDistance = 3f;


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
            // 공중 몬스터 고정 높이
            spawnY = 2f;
        }
        else
        {
            // 지상 몬스터는 Raycast로 지형 감지
            Vector2 pos = new Vector2(randomX, 10f);
            RaycastHit2D rayHit = Physics2D.Raycast(pos, Vector2.down, 15f, LayerMask.GetMask("Ground")); // 처음 감지 위치, 아래 방향, 15만큼 거리, Ground만 인식

            if (rayHit.collider != null)
            {
                spawnY = rayHit.point.y;
            }
            else
            {
                // 지형을 못 찾으면 기본값에 생성
                spawnY = -2f;
            }
        }

        Vector3 createPos = new Vector3(randomX, spawnY, 0);

        if (Vector2.Distance(player.position, createPos) >= minDistance) ;
        {
            Instantiate(prefab, createPos, Quaternion.identity, monster);
            return;
        }
    }
}