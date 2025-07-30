using UnityEngine;

public class Crab : MonsterManager
{
    public override void Init()
    {
        monsterHp = 3f;
        monsterMaxHp = monsterHp;
        moveSpeed = 2f;
        attackDamage = 4f;

        attackAnimations = new string[] { "Attack", "Attack2", "Attack3" };
    }

    public override float AttackDamage()
    {
        return attackDamage;
    }
}
