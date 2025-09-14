using UnityEngine;

namespace DemonBoss
{
    public class CastSpellState : IDemonState
    {
        private BossStateMachine bsm;

        public CastSpellState(BossStateMachine bsm)
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
