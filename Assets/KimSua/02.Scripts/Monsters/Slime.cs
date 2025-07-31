using UnityEngine;

public class Slime : MonsterManager
{
    public override void Init()
    {
        monsterHp = 2f;
        monsterMaxHp = monsterHp;
        moveSpeed = 1.5f;
        attackDamage = 2f;
    }

    public override float AttackDamage()
    {
        return attackDamage;
    }
}
