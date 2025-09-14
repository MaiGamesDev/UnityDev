using UnityEngine;

namespace DemonBoss
{
    public class SmashState : IDemonState
    {
        private BossStateMachine bsm;

        public SmashState(BossStateMachine bsm)
        {
            this.bsm = bsm;
        }

        public void OnEnter()
        {

        }

        public void OnExit()
        {

        }

        public void OnState()
        {
            bsm.LookTarget();

        }
    }
}