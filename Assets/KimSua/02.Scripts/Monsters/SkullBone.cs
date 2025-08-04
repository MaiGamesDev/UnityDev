using UnityEngine;

public class SkullBone : MonsterManager
{
    public override void Init()
    {
        monsterHp = 10f;
        monsterMaxHp = monsterHp;
        moveSpeed = 4.5f;
        attackDamage = 4f;
    }

    public override float AttackDamage()
    {
        return attackDamage;
    }
}
