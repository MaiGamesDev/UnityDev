using BossAllStatus;
using UnityEngine;

namespace DemonBoss
{
    public class SmashState : IDemonState
    {
        private BossStateMachine bsm;
        private BossDemonAbility demonAbil;
        private AttackTrigger attackTrigger;

        public SmashState(BossStateMachine bsm)
        {
            this.bsm = bsm;
        }

        public void OnEnter()
        {

        }
        public void OnState()
        {
            bsm.LookTarget();

        }

        public void OnExit()
        {

        }


        public void OnAnimationEventTrigger()
        {
            attackTrigger.EnableAttack(demonAbil.smashDamage);
        }
    }
}