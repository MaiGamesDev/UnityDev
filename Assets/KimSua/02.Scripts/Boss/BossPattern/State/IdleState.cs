using UnityEngine;

public class IdleState : IBossState
{
    private float idleTime;

    public void StateEnter(BossController boss)
    {
        boss.isMove = false;
        boss.timer = 0f;
        idleTime = Random.Range(1f, 5f);
        boss.GetComponent<Animator>().SetBool("isWalk", false);
    }

    public void StateExecute(BossController boss)
    {
        boss.timer += Time.deltaTime;
        if (boss.timer >= idleTime)
        {
            boss.isMove = true;

            if (boss.targetDist <= boss.traceDist)
            {
                boss.ChangeState<TraceState>();
            }
            else
            {
                boss.ChangeState<WalkState>();
            }
        }
    }

    public void StateExit(BossController boss) { }
}
