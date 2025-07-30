using UnityEngine;

public class Rat : MonsterManager
{
    public override void Init()
    {
        monsterHp = 2f;
        monsterMaxHp = monsterHp;
        moveSpeed = 2f;
        attackDamage = 2.5f;
    }

    public override float AttackDamage()
    {
        return attackDamage;
    }
}
