using UnityEngine;

namespace DemonBoss
{
    public class HitState : IDemonState
    {
        private BossStateMachine bsm;

        public HitState(BossStateMachine bsm)
        {
            this.bsm = bsm;
        }

        public void OnEnter()
        {
            bsm.anim.SetBool("isAttack", false);
            bsm.anim.SetBool("isWalk", false);
        }

        public void OnState()
        {
            //if (KnightController.)
            bsm.anim.SetBool("isHIt", true);
            // 기사 캐릭터 공격했을 때 메서드 들고와서 HP깍아야함
        }
        public void OnExit()
        {

        }
    }
}
