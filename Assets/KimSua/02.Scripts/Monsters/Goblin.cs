using UnityEngine;

public class Goblin : MonsterManager
{
    public override void Init()
    {
        monsterHp = 5f;
        monsterMaxHp = monsterHp;
        moveSpeed = 1.5f;
        attackDamage = 3f;
    }

    public override float AttackDamage()
    {
        return attackDamage;
    }
}
