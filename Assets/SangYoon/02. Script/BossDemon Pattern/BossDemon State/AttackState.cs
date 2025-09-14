using UnityEngine;

namespace DemonBoss
{
    public class AttackState : IDemonState
    {
        public readonly float attackDemage = 10f; 
        private BossStateMachine bsm;
        public AttackState(BossStateMachine bsm)
        {
            this.bsm = bsm;
        }

        public void OnEnter()
        {
            bsm.anim.SetBool("isWalk", false);
            bsm.anim.SetBool("isAttack", true);
        }

        public void OnState()
        {

            bsm.LookTarget();
            float dist = Vector2.Distance(bsm.transform.position, bsm.target.position);

            if (dist > bsm.attackDist)
            {
                bsm.ChangeState<IdleState>();
            }
        }

        public void OnExit()
        {

        }
    }
}
