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

        public void OnState()
        {
            bsm.LookTarget();

            // 상호작용이 안되도록 하면 될듯?
        }
        public void OnExit()
        {

        }

    }
}
