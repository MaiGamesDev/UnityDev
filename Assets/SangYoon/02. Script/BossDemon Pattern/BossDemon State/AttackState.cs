using UnityEngine;
using BossAllStatus;
namespace DemonBoss
{
    public class AttackState : IDemonState
    {
        private BossStateMachine bsm;
        private BossDemonAbility demonAbil;

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

            //
            float dist = Vector2.Distance(bsm.transform.position, bsm.target.position);

            if (dist > bsm.attackDist)
            {
                bsm.ChangeState<IdleState>();
                BossAttack("Player", demonAbil.attackDamage);
            }
        }

        public void OnExit()
        {

        }

        public void BossAttack(Collider2D other, float damage)
        {
            if (other.CompareTag("Player"))
            {
                GameManager.Instance.hp -= damage;
            }
        }
    }
}
