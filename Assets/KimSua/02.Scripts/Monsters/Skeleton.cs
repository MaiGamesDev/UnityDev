using UnityEngine;

public class Skeleton : MonsterManager
{
    public override void Init()
    {
        monsterHp = 10f;
        monsterMaxHp = monsterHp;
        moveSpeed = 0.8f;
        attackDamage = 8f;

        attackAnimations = new string[] { "Attack", "Attack2" };
    }

    public override float AttackDamage()
    {
        return attackDamage;
    }
}
