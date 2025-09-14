using UnityEngine;

namespace BossAllStatus
{
    public class BossDemonAbility : MonoBehaviour
    {
        public float hp = 500f;

        [Header("Attack & Skills Damage")]
        public readonly float attackDamage = 10f;
        public readonly float smashDamage = 30f;
        public readonly float fireBreathDamage = 20f;
        public readonly float fireBallDamage = 50f;

    }

    public class BossDemonSkillUtility : MonoBehaviour
    {

        [Header("Skill Cooldowns")]
        public readonly float smashCooldown = 6;
        public readonly float fireBreathCooldown = 10;
        public readonly float fireBallCooldown = 17;

        [Header("Skill Cast Times")]
        public readonly float smashCastTime = 1.05f;
        public readonly float fireBreathCastTime = 1.07f;
        public readonly float fireBallCastTime = 9f;

        [Header("Skill Range")]
        public readonly float smashRange = 1.05f;
        public readonly float fireBreathRange = 1.07f;
        public readonly float fireBallRange = 9f;

        public readonly float moveSpeed = 1f;


    }

    public class UseSkill : MonoBehaviour
    {
        private BossStateMachine bsm;

        public void SkillOnReady(float cooldown)
        {
            bsm.stateTime += Time.deltaTime;

            if (cooldown <= 0)
            {

            }
        }
    }
}