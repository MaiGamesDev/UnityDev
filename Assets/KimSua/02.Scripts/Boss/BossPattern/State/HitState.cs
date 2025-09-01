using System.Collections;
using UnityEngine;

public class HitState : IBossState
{
    public void StateEnter(BossController boss)
    {
        boss.isMove = false;
        boss.GetComponent<Animator>().SetBool("isWalk", false);
        boss.GetComponent<Animator>().SetTrigger("Hurt");
        boss.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        boss.StartCoroutine(HitRoutine(boss));
    }

    public void StateExecute(BossController boss) { }

    public void StateExit(BossController boss)
    {
        boss.isMove = true;
    }

    private IEnumerator HitRoutine(BossController boss)
    {
        yield return new WaitForSeconds(1f);
        boss.ChangeState<IdleState>();
    }
}
