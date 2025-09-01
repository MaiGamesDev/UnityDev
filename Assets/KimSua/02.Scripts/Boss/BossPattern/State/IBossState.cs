using UnityEngine;

public interface IBossState
{
    void StateEnter(BossController boss);
    void StateExecute(BossController boss);
    void StateExit(BossController boss);
}
