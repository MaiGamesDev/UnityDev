using BossAllStatus;
using UnityEngine;

namespace DemonBoss
{
    public class CastSpellState : IDemonState
    {
        private BossStateMachine bsm;
        private BossDemonAbility demonAbil;
        private AttackTrigger attackTrigger;
        private GameObject fireBall;  // 파이어볼 프리팹

        public CastSpellState(BossStateMachine bsm)
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
            fireBall.Instantiate("fireBall", bsm.fireBallPos, Quaternion.identity);
            attackTrigger.EnableAttack(demonAbil.fireBallDamage);
        }
    }
}
