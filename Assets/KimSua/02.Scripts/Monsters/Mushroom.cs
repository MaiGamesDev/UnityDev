using UnityEngine;

public class Mushroom : MonsterManager
{
    public override void Init()
    {
        monsterHp = 7f;
        moveSpeed = 1f;
        attackDamage = 3f;
    }

    public override float AttackDamage()
    {
        return attackDamage;
    }
}
