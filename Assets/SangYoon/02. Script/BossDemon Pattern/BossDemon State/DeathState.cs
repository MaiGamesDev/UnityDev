using UnityEngine;

namespace DemonBoss
{
    public class DeathState : IDemonState
    {
        private BossStateMachine bsm;

        public DeathState(BossStateMachine bsm)
        {
            this.bsm = bsm;
        }

        public void OnEnter()
        {
            bsm.anim.SetBool("isAttack", false);
            bsm.anim.SetBool("isWalk", false);
            bsm.anim.SetBool("isHIt", false);

            bsm.anim.SetTrigger("Death");
        }

        public void OnExit()
        {

        }

        public void OnState()
        {

        }
    }
}
