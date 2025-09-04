using System.Collections;
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

    protected override IEnumerator DeathRoutine()
    {
        SoundManager.Instance.PlaySound(sndDie);
        stateType = StateType.Death;
        isDead = true;

        animator.SetTrigger("Death");
        // yield return new WaitForSeconds(GetAnimLegnth("Death"));
        yield return null;

        monsterRb.gravityScale = 1f;

        item.DropItem(transform.position, gameObject);

        Destroy(gameObject, 0.5f); // EvilWizard는 풀로 반환하지 않고 바로 파괴
        gameObject.layer = LayerMask.NameToLayer("DeadMonster");
    }
}
