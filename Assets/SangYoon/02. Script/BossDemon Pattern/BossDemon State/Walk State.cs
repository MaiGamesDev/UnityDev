using System;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace DemonBoss
{
    public class WalkState : IDemonState
    {
        private BossStateMachine bsm;

        public WalkState(BossStateMachine bsm)
        {
            this.bsm = bsm;
        }

        public void OnEnter()
        {
            bsm.anim.SetBool("isAttack", false);
            bsm.anim.SetBool("isWalk", true);
        }

        public void OnState()
        {
            bsm.LookTarget();

            float dist = Vector2.Distance(bsm.transform.position, bsm.target.position);

            Vector3 dir = (bsm.target.position - bsm.transform.position).normalized;
            bsm.transform.position += dir * bsm.moveSpeed * Time.deltaTime;

            bsm.stateTime += Time.deltaTime;

            if (dist <= bsm.attackDist)
            {
                bsm.ChangeState<AttackState>();
            }
        }

        public void OnExit()
        {
        }
    }
}
