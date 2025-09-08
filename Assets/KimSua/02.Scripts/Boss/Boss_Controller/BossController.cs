using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour, IBossDefaultPattern
{
    #region 멤버변수
    [Header("Boss Stats")]
    public float hp { get; set; }
    public float attackDamage { get; set; }
    public float moveSpeed { get; set; }
    public float currHp;

    [Header("Dist Settings")]
    public float traceDist = 8f;
    public float attackDist = 3f;
    public float attackCooldown = 2f;

    [Header("Audio")]
    [SerializeField] protected AudioClip sndHit;
    [SerializeField] protected AudioClip sndDie;

    #region Components
    protected Animator animator;
    protected Rigidbody2D bossRb;
    protected Collider2D bossColl;
    protected ItemDropSpawner item;
    public Transform target;
    #endregion

    private IBossState currentState;
    // key : 상태 클래스 타입, value : 실제 상태
    private Dictionary<Type, IBossState> states;

    public IBossAttackStrategy attackStrategy;
    public IBossMoveStrategy moveStrategy;

    #region State Variables
    public float targetDist;
    public float moveDir;
    public float timer;
    public float lastAttackTime;

    public bool isMove;
    public bool isAttack;
    private bool isPlayerDead;
    private bool isDead;
    public bool isFacingRight = false;
    #endregion
    #endregion

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        bossRb = GetComponent<Rigidbody2D>();
        bossColl = GetComponent<Collider2D>();
        item = FindFirstObjectByType<ItemDropSpawner>();
        target = GameObject.FindGameObjectWithTag("Player")?.transform;

        InitStates();
        SetupStrategies();
    }

    protected virtual void Start()
    {
        SetupStats();
        currHp = hp;
        ChangeState<IdleState>();
        StartCoroutine(FindPlayerRoutine());
    }

    protected virtual void SetupStats()
    {
        // 기본 스탯 설정
        hp = 50f;
        attackDamage = 10f;
        moveSpeed = 1f;
    }

    protected virtual void SetupStrategies()
    {
        moveStrategy = new MoveStrategy();
    }

    protected virtual void InitStates()
    {
        states = new Dictionary<Type, IBossState>
        {
            {typeof(IdleState), new IdleState() },
            {typeof(WalkState), new WalkState() },
            {typeof(TraceState), new TraceState() },
            {typeof(AttackState), new AttackState() },
            {typeof(HitState), new HitState() }
        };
    }

    void Update()
    {
        if (isDead) return;

        targetDist = Vector3.Distance(transform.position, target.position);
        moveStrategy?.CheckBoundary(this);
        currentState?.StateExecute(this);
    }

    public void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void DisableSelf()
    {
        gameObject.SetActive(false);
    }

    // 반드시 IBossState 인터페이스를 구현한 클래스
    public void ChangeState<T>() where T : IBossState
    {
        if (states == null)
        {
            InitStates();
        }

        // 딕셔너리에 key값(타입)이 존재하면 true 반환, 해당 value를 out 변수에 담음
        if (states.TryGetValue(typeof(T), out IBossState newState))
        {
            currentState?.StateExit(this);
            currentState = newState;
            currentState.StateEnter(this);
        }
    }

    protected virtual IEnumerator FindPlayerRoutine()
    {
        while (!isDead && target != null)
        {
            yield return null;

            // State 전환 로직
            if (currentState is IdleState || currentState is WalkState)
            {
                if (targetDist <= traceDist)
                {
                    ChangeState<TraceState>();
                }
            }
            else if (currentState is TraceState)
            {
                if (targetDist > traceDist)
                {
                    ChangeState<WalkState>();
                }
            }

            // 공격 조건 체크
            if (targetDist <= attackDist && !(currentState is AttackState) &&
                Time.time - lastAttackTime >= attackCooldown)
            {
                lastAttackTime = Time.time;
                ChangeState<AttackState>();
            }
        }
    }

    public void Idle()
    {
        if (currentState is IdleState)
            currentState.StateExecute(this);
    }

    public void Walk()
    {
        if (currentState is WalkState)
            currentState.StateExecute(this);
    }

    public void Trace()
    {
        if (currentState is TraceState)
            currentState.StateExecute(this);
    }

    #region Hit

    public virtual void Hit(float damage)
    {
        if (isDead) return;

        SoundManager.Instance?.PlaySound(sndHit);
        currHp -= damage;
        UIManager.Instance?.SetHpEnemy(currHp, hp);

        if (currHp <= 0)
        {
            Death();
        }
        else
        {
            ChangeState<HitState>();
        }
    }

    public void Death()
    {
        isDead = true;

        SoundManager.Instance.PlaySound(sndDie); // Die 사운드

        animator.SetTrigger("Death");
        bossColl.enabled = false;
        bossRb.gravityScale = 0f;

        item.DropItem(transform.position, gameObject);

        Destroy(gameObject, 1f);
        gameObject.layer = LayerMask.NameToLayer("DeadMonster");
    }

    #endregion   

    public void DefaultAttack()
    {
        attackStrategy?.Execute(this);
    }
}
