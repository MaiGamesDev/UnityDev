using UnityEngine;

public class TraceState : IBossState
{
    public void StateEnter(BossController boss)
    {
        boss.GetComponent<Animator>().SetBool("isWalk", true);
    }

    public void StateExecute(BossController boss)
    {
        var target = boss.target;
        if (target == null) return;

        var targetDir = (target.position - boss.transform.position).normalized;
        boss.moveStrategy?.Move(boss, Vector3.right * targetDir.x);

        var direction = targetDir.x > 0 ? 1 : -1;
        if ((direction > 0 && !boss.isFacingRight) || (direction < 0 && boss.isFacingRight))
            boss.Flip();

        boss.moveDir = direction;
    }

    public void StateExit(BossController boss) { }
}
