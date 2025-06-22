using UnityEngine;

public class FlyingEye : MonsterManager
{
    public override void Init()
    {
        monsterHp = 2f;
        moveSpeed = 2f;
        attackDamage = 1f;
    }

    public override float AttackDamage()
    {
        return attackDamage;
    }
}
