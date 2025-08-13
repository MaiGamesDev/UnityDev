using UnityEngine;

public class Canine : MonsterManager
{
    public override void Init()
    {
        monsterHp = 20f;
        monsterMaxHp = monsterHp;
        moveSpeed = 5f;
        attackDamage = 3f;
    }

    public override float AttackDamage()
    {
        return attackDamage;
    }
}