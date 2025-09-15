using UnityEngine;

namespace DemonBoss
{
    /// <summary>
    /// 보스의 
    /// </summary>
    public interface IDemonState
    {
        void OnEnter();
        void OnState();
        void OnExit();
        
        // 공격관련 애니메이션 트리거 충돌 판정
        void OnAnimationEventTrigger();
    }

}
