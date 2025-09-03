using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BossDemon : MonoBehaviour, IBossDefaultPattern
{
    public enum BossState { Idle, Walk, DefaultAttack, }
    public BossState bossState;

    public float hp { get; set; } = 500f;
    public float attackDamage { get; set; } = 10f;
    public float moveSpeed { get; set; } = 1.4f;
    public float skillDamage { get; private set; }

    public bool isIdle = true;
    public bool isWalk = true;
    public bool isAttack = true;

    private float attackDist = 3.85f;

    public float idleTime = 1f;
    private float walkTime = 3f;
    private float attackTime = 1f;

    private float lastAttackEndTime = Mathf.NegativeInfinity; // -infinity로 언제든 일반 공격이 가능하게끔 하기위함
    private float stateTimer;

    public Animator anim;
    private Rigidbody2D DemonRb;
    private DemonSummon damonSummon;

    [SerializeField] private Transform target;

    //protected virtual void Init(float hp, float attackDamage, float moveSpeed)
    //{
    //    this.hp = hp;
    //    this.attackDamage = attackDamage;
    //    this.moveSpeed = moveSpeed;
    //}

    private void Awake()
    {
        DemonRb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        damonSummon = GetComponent<DemonSummon>();

        anim.enabled = false;
    }
    private void Start()
    {
    }

    void Update()
    {
        StateTransitions();

        switch (bossState)
        {
            case BossState.Idle:
                Idle();
                break;

            case BossState.Walk:
                Walk();
                break;

            case BossState.DefaultAttack:
                DefaultAttack();
                break;
        }
    }

    void StateTransitions()
    {
        float dist = Vector2.Distance(transform.position, target.position); // 공격 범위를 판별하기 위함

        bool canAttack = (Time.time - lastAttackEndTime) >= attackTime;

        if (target != null && dist <= attackDist && canAttack && bossState != BossState.DefaultAttack)
        {
            ChangeState(BossState.DefaultAttack);
            return;
        }
        if (target != null && dist > attackDist && bossState != BossState.Walk && bossState != BossState.DefaultAttack)
        {
            ChangeState(BossState.Walk);
            //stateTimer = Time.deltaTime; // 유사시 바로 사용할거라 주석 처리해두었음
            return;
        }
        if (target != null && stateTimer >= walkTime && bossState != BossState.Idle && bossState != BossState.DefaultAttack)
        {
            ChangeState(BossState.Idle);
            //stateTimer = 0; // 유사시 바로 사용할거라 주석 처리해두었음
            return;
        }
    }

    public void ChangeState(BossState newState)
    {
        if (bossState != newState)
            bossState = newState;
    }
    public void Idle()
    {
        LookTarget();
        anim.SetBool("isWalk", false);
        anim.SetBool("isAttack", false);
        anim.SetTrigger("Idle");

        stateTimer += Time.deltaTime;
        if (stateTimer >= idleTime)
        {
            stateTimer = 0;
        }
    }

    public void Walk()
    {
        LookTarget();

        // 움직이는 로직
        Vector3 dir = (target.position - transform.position).normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;

        anim.SetBool("isWalk", true);
        anim.SetBool("isAttack", false);

        stateTimer += Time.deltaTime;
    }


    public void DefaultAttack()
    {
        LookTarget();
        anim.SetBool("isWalk", false);
        anim.SetBool("isAttack", true);

        stateTimer += Time.deltaTime;
        if (stateTimer >= attackTime)
        {
            anim.SetBool("isAttack", false);
            lastAttackEndTime = Time.time;

            ChangeState(BossState.Idle);
        }
    }
    //IEnumerator AttackRoutine()
    //{

    //}

    public void Hit(float damage)
    {

    }

    public void Death()
    {

    }

    /// <summary>
    /// 보스 방향 전환
    /// </summary>
    private void LookTarget()
    {
        Vector3 product = target.position - transform.position;
        Vector3 scale = transform.localScale;
        scale.x = product.x > 0 ? -1 : 1;
        transform.localScale = scale;
    }
}