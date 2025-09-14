using BossAllStatus;
using UnityEngine;

namespace DemonBoss
{
    public class HitState : IDemonState
    {
        private BossStateMachine bsm;
        private KnightController knight;
        private BossDemonAbility demonAbil;

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
            bsm.LookTarget();
            TakeDamage(knight.defaultDamage);
        }
        public void OnExit()
        {

        }

        private void TakeDamage(float damage)
        {
            if (demonAbil.hp > 0)
            {
                bsm.anim.SetBool("isHIt", true);
                demonAbil.hp -= damage;
            }
            else
            {
                bsm.anim.SetBool("isHIt", false);
                bsm.ChangeState<DeathState>();
            }
        }
    }
}
