using System.Collections;
using UnityEngine;

public class AttackState : IBossState
{
    public void StateEnter(BossController boss)
    {
        boss.isMove = false;
        boss.isAttack = true;
        boss.GetComponent<Animator>().SetBool("isWalk", false);
        boss.StartCoroutine(AttackRoutine(boss));
    }

    public virtual void StateExecute(BossController boss)
    {
        boss.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
    }

    public void StateExit(BossController boss)
    {
        boss.isMove = true;
        boss.isAttack = false;
    }

    private IEnumerator AttackRoutine(BossController boss)
    {
        yield return null;

        // 여기서 전략(Strategy) 실행

        boss.attackStrategy?.Execute(boss);

        boss.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(1f);
        boss.ChangeState<IdleState>();
    }
}
