using UnityEngine;

public class MoveStrategy : IBossMoveStrategy
{
    private float minX = -8f;
    private float maxX = 22f;

    public void Move(BossController boss, Vector3 direction)
    {
        boss.transform.position += direction * boss.moveSpeed * Time.deltaTime;
    }

    public void CheckBoundary(BossController boss)
    {
        var pos = boss.transform.position;

        if (pos.x >= maxX)
        {
            boss.transform.position = new Vector3(maxX, pos.y, pos.z);
            boss.moveDir = -1;
            if (boss.isFacingRight) boss.Flip();
        }
        else if (pos.x <= minX)
        {
            boss.transform.position = new Vector3(minX, pos.y, pos.z);
            boss.moveDir = 1;
            if (!boss.isFacingRight) boss.Flip();
        }
    }
}
