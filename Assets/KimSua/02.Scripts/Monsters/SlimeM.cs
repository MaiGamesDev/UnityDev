using System.Collections;
using UnityEngine;

public class SlimeM : MonsterManager
{
    public override void Init()
    {
        monsterHp = 15f;
        monsterMaxHp = monsterHp;
        moveSpeed = 3f;
        attackDamage = 6f;
    }

    public override float AttackDamage()
    {
        return attackDamage;
    }
}
