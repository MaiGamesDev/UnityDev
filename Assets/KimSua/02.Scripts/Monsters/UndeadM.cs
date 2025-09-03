using System.Collections;
using UnityEngine;

public class UndeadM : MonsterManager
{
    [SerializeField] private Summon summonPrefab;
    [SerializeField] private Transform summonSpawnPoint;

    public override void Init()
    {
        monsterHp = 20f;
        monsterMaxHp = monsterHp;
        moveSpeed = 5f;
        attackDamage = 6f;
    }

    public override float AttackDamage()
    {
        return attackDamage;
    }

    protected override void Death()
    {
        base.Death();

        if (summonPrefab != null)
        {
            int summonCount = Random.Range(1, 4);
            Transform player = GameObject.FindGameObjectWithTag("Player").transform;

            for (int i = 0; i < summonCount; i++)
            {
                float offsetX = Random.Range(-5f, 5f);
                Vector3 spawnPos = (summonSpawnPoint ? summonSpawnPoint.position : transform.position) + new Vector3(offsetX, 1f, 0);
                Summon newSummon = Instantiate(summonPrefab, spawnPos, Quaternion.identity);                
            }
        }
    }
}
