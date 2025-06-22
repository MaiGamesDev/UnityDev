using UnityEngine;

public class Skeleton : MonsterManager
{
    public override void Init()
    {
        monsterHp = 10f;
        moveSpeed = 0.5f;
        attackDamage = 8f;
    }

    public override float AttackDamage()
    {
        return attackDamage;
    }
}
