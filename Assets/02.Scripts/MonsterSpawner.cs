using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> monsters = new List<GameObject>();
    [SerializeField] private Transform monster;

    private int monsterCount;

    private void Awake()
    {
        monsters = Resources.LoadAll<GameObject>("Monsters").ToList();
    }

    IEnumerator Start()
    {
        while (monsterCount < 6)
        {
            yield return new WaitForSeconds(1f);

                var randomIndex = Random.Range(0, monsters.Count);
                GameObject prefab = monsters[randomIndex];
                var randomX = Random.Range(-2, 9);

                // Fly 태그일 경우 공중 위치, 아닐 경우 지상 위치
                float spawnY = prefab.CompareTag("Fly") ? 2f : -2f;
                Vector3 createPos = new Vector3(randomX, spawnY, 0);

                Instantiate(prefab, createPos, Quaternion.identity, monster);

                monsterCount++;                        
        }        
    }
}
