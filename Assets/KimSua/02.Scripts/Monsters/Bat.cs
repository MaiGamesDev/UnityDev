using UnityEngine;

public class Bat : MonsterManager
{
    public override void Init()
    {
        monsterHp = 11f;
        monsterMaxHp = monsterHp;
        moveSpeed = 4f;
        attackDamage = 3f;
    }

    public override float AttackDamage()
    {
        return attackDamage;
    }
}
