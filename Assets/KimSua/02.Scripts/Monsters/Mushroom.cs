using UnityEngine;

public class Mushroom : MonsterManager
{
    public override void Init()
    {
        monsterHp = 7f;
        monsterMaxHp = monsterHp;
        moveSpeed = 1f;
        attackDamage = 3f;

        attackAnimations = new string[] { "Attack", "Attack2" };
    }

    public override float AttackDamage()
    {
        return attackDamage;
    }
}
