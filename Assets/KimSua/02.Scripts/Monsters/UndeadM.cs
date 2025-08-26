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

        attackAnimations = new string[] { "Attack", "Spawn" };
    }

    public override float AttackDamage()
    {
        return attackDamage;
    }

    protected override IEnumerator AttackRoutine()
    {
        isAttacking = true;
        isMove = false;
        animator.SetBool("isRun", false);
        monsterRb.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(0.1f);

        if (summonPrefab != null)
        {
            Vector3 spawnPos = summonSpawnPoint ? summonSpawnPoint.position : transform.position;
            Summon newSummon = Instantiate(summonPrefab, spawnPos, Quaternion.identity);
            newSummon.Init(GameObject.FindGameObjectWithTag("Player").transform);
        }

        yield return new WaitForSeconds(0.5f);

        isAttacking = false;
        isMove = true;
        animator.SetBool("isRun", true);
        ChangeStateType(StateType.Idle);
    }
}
