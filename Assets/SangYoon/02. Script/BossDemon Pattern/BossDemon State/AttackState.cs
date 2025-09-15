using UnityEngine;
using BossAllStatus;
namespace DemonBoss
{
    public class AttackState : IDemonState
    {
        private BossStateMachine bsm;
        private BossDemonAbility demonAbil;
        private AttackTrigger attackTrigger;

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
            bsm.anim.SetBool("isAttack", false);
        }

        public void OnAnimationEventTrigger()
        {
            attackTrigger.EnableAttack(demonAbil.attackDamage);
        }
    }
}