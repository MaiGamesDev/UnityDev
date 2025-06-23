using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> monsters = new List<GameObject>();
    [SerializeField] private Transform monster;


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
        var randomX = Random.Range(-2, 9);
        float spawnY;

        if (prefab.CompareTag("Fly"))
        {
            // ���� ���� ���� ����
            spawnY = 2f;
        }
        else
        {
            // ���� ���ʹ� Raycast�� ���� ����
            Vector2 pos = new Vector2(randomX, 10f);
            RaycastHit2D rayHit = Physics2D.Raycast(pos, Vector2.down, 20f, LayerMask.GetMask("Ground")); // ó�� ���� ��ġ, �Ʒ� ����, 20��ŭ �Ÿ�, Ground�� �ν�

            if (rayHit.collider != null)
            {
                spawnY = rayHit.point.y;
            }
            else
            {
                // ������ �� ã���� �⺻���� ����
                spawnY = -2f;
                Debug.Log("���� ã�� ���߽��ϴ�.");
            }
        }

        Vector3 createPos = new Vector3(randomX, spawnY, 0);
        Instantiate(prefab, createPos, Quaternion.identity, monster);        
    }    
}