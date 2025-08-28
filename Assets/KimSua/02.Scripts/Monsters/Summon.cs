using System.Collections;
using UnityEngine;
using static UnityEditor.Progress;

public class Summon : MonsterManager
{
    [SerializeField] private int summonMoveSpeed;

    public override void Init()
    {
        monsterHp = 10f;
        monsterMaxHp = monsterHp;

        summonMoveSpeed = Random.Range(3, 6);

        moveSpeed = summonMoveSpeed;
        attackDamage = 4f;

        ChangeStateType(StateType.Trace);
        animator.SetTrigger("Spawn");
    }

    public override float AttackDamage()
    {
        return attackDamage;
    }

    protected override void ChangeStateType(StateType newState)
    {
        if (newState == StateType.Trace || newState == StateType.Death)
            base.ChangeStateType(newState);
    }

    protected override void Death()
    {
        monsterRb.bodyType = RigidbodyType2D.Static;
        base.Death();
    }
}
