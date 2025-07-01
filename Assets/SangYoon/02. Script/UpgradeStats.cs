using UnityEngine;

public class UpgradeStats : KnightController
{
    public Animator animator;
    // For this field use when Knight stats upgrade
    //-----------------------------------------------------------------------------------------
    private float upDamage = 1; // 승수에 따라 몬스터 체력 구현이 뒷받침 되어야함
    private float upAttackSpeed = 0.1f; // 승수는 0.1 ~ 0.03으로 구현해야함 (기본 공격 속도가 1이기 때문)
    private float upHp = 2;
    //-----------------------------------------------------------------------------------------

    public bool isHpUpgrade;
    public bool isDamageUpgrade;
    public bool isAttackSpeedUpgrade;

    KnightController knightController;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Upgrade()
    {
        if (isHpUpgrade)
        {
            Debug.Log("HP 증가!");
            knightController.defaultHp += upHp;
        }

        if (isDamageUpgrade)
        {
            Debug.Log("공격력 증가!");
            knightController.defaultDamage += upDamage;
        }

        if (isAttackSpeedUpgrade)
        {
            Debug.Log("공속 증가!");
            knightController.hitboxAttackSpeed -= upAttackSpeed;
            animator.SetFloat("AttackSpeed", animationAttackSpeed + upAttackSpeed);
        }
    }
}
