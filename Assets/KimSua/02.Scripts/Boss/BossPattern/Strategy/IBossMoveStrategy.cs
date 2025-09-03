using UnityEngine;

public interface IBossMoveStrategy
{
    void Move(BossController boss, Vector3 dir);
    void CheckBoundary(BossController boss);
}
