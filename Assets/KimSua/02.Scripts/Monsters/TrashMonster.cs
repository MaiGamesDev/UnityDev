using System.Collections;
using UnityEngine;

public class TrashMonster : MonsterManager
{
    public override void Init()
    {
        monsterHp = 15f;
        monsterMaxHp = monsterHp;
        moveSpeed = 4f;
        attackDamage = 6f;
    }

    public override float AttackDamage()
    {
        return attackDamage;
    }
}
