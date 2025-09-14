using UnityEngine;

namespace BossAllStatus
{
    public class BossDemonAbility : MonoBehaviour
    {
        [SerializeField] private float HP = 500f;

        [Header("Attack & Skills Damage")]
        [SerializeField] private float AttackDamage = 10f;
        [SerializeField] private float SmashDamage = 30f;
        [SerializeField] private float FireBreathDamage = 20f;
        [SerializeField] private float FireBallDamage = 50f;

    }

    public class BossDemonSkillUtility : MonoBehaviour
    {

        [Header("Skill Cooldowns")]
        [SerializeField] private float SmashCooldown = 6;
        [SerializeField] private float FireBreathCooldown = 10;
        [SerializeField] private float FireBallCooldown = 17;

        [Header("Skill Cast Times")]
        [SerializeField] private float SmashCastTime = 1.05f;
        [SerializeField] private float FireBreathCastTime = 1.07f;
        [SerializeField] private float FireBallCastTime = 9f;
        
        [SerializeField] private float moveSpeed = 1f;

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
            return;
        }

        public void CastSkill()
        {

        }
    }
}