using UnityEngine;

public class EvilWizard : MonsterManager
{
    public override void Init()
    {
        monsterHp = 20f;
        monsterMaxHp = monsterHp;
        moveSpeed = 1.5f;
        attackDamage = 3f;
    }

    public override float AttackDamage()
    {
        return attackDamage;
    }
}
