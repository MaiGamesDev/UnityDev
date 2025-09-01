using UnityEngine;

public class WalkState : IBossState
{
    private float walkTime;

    public void StateEnter(BossController boss)
    {
        boss.moveDir = Random.Range(0, 2) == 1 ? 1 : -1;
        walkTime = Random.Range(1f, 5f);
        boss.timer = 0f;

        if ((boss.moveDir > 0 && !boss.isFacingRight) || (boss.moveDir < 0 && boss.isFacingRight))
            boss.Flip();

        boss.GetComponent<Animator>().SetBool("isWalk", true);
    }

    public void StateExecute(BossController boss)
    {
        if (boss.isMove)
        {
            boss.moveStrategy?.Move(boss, Vector3.right * boss.moveDir);

            boss.timer += Time.deltaTime;
            if (boss.timer >= walkTime)
            {
                boss.ChangeState<IdleState>();
            }
        }
    }

    public void StateExit(BossController boss) { }
}
