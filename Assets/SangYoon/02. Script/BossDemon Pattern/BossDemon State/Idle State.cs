using System;
using UnityEngine;

namespace DemonBoss
{
    public class IdleState : IDemonState
    {
        private BossStateMachine bsm;
        public IdleState(BossStateMachine bsm)
        {
            this.bsm = bsm;
        }

        
        public void OnEnter()
        {
            bsm.anim.SetBool("isWalk", false);
            bsm.anim.SetBool("isAttack", false);
            bsm.anim.SetTrigger("Idle");
        }

        public void OnState()
        {
            bsm.stateTime += Time.deltaTime;
            bsm.LookTarget();

            if (bsm.stateTime >= bsm.idleTime)
            {
                bsm.stateTime = 0;
                bsm.ChangeState<WalkState>();
            }

        }

        public void OnExit()
        {
            bsm.idleTime = 0.5f;
        }

        public void OnAnimationEventTrigger()
        {

        }
    }
}
