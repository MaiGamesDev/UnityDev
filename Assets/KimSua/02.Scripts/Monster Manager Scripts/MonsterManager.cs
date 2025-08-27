using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static BossBringer;
using static UnityEditor.PlayerSettings;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public abstract class MonsterManager : MonoBehaviour
{
    #region 멤버변수
    public enum StateType { Idle, Move, Trace, Attack, Death }
    public StateType stateType;

    protected SpriteRenderer sRenderer;
    protected Animator animator;
    protected Rigidbody2D monsterRb;
    protected Collider2D monsterColl;
    protected ItemDropSpawner item;
    Transform target;
    [SerializeField] private GameObject attackHitbox;

    protected bool canFly;
    private bool isFacingRight = false;
    protected bool isMove;
    protected bool isTrace;
    [HideInInspector] public bool isAttacking;
    protected bool isDead = false;
    bool isPlayerDead = false;


    private float minFlightHeight = -2.35f;
    [SerializeField] protected float moveSpeed = 1f;
    [SerializeField] private float traceRange = 5f;
    [SerializeField] private float attackRange = 2f;
    public float attackDamage = 3f;

    public float monsterHp = 10f;
    protected float monsterMaxHp;
    protected string[] attackAnimations = { "Attack" };

    private float idleTime;
    private float walkTime;
    private float targetDist;

    private float moveDir;
    private Vector2 moveVector;
    private float timer;

    [SerializeField] private AudioClip sndHit;
    [SerializeField] private AudioClip sndDie;

    public abstract void Init();
    #endregion
    // ----------------------------------------------------------------------------------------

    #region Default
    private void Awake()
    {
        sRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        monsterRb = GetComponent<Rigidbody2D>();
        monsterColl = GetComponent<Collider2D>();

        item = FindFirstObjectByType<ItemDropSpawner>();
        target = GameObject.FindGameObjectWithTag("Player").transform;

        canFly = gameObject.CompareTag("Fly");
    }

    void Start()
    {
        Init();
        StartCoroutine(FindPlayerRoutine());
    }

    void Update()
    {
        isPlayerDead = KnightController.isDead;
        if (isPlayerDead || isDead) return;

        CheckBoundary(); // x범위 제한

        switch (stateType)
        {
            case StateType.Idle:
                Idle();
                break;
            case StateType.Move:
                Move();
                break;
            case StateType.Trace:
                Trace();
                break;
            case StateType.Attack:
                Attack();
                break;
            case StateType.Death:
                Death();
                break;
        }
    }

    private void FixedUpdate()
    {
        if (isDead) return;

        if (isMove)
        {
            Vector2 velocity = new Vector2(moveVector.x * moveSpeed, moveVector.y * moveSpeed);

            if (canFly && transform.position.y <= minFlightHeight)
            {
                velocity.y = 0;
            }

            monsterRb.linearVelocity = velocity;
        }
        else
            monsterRb.linearVelocity = Vector2.zero;
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void CheckBoundary()
    {
        if (transform.position.x >= 8f)
        {
            transform.position = new Vector3(8f, transform.position.y, transform.position.z);
            moveDir = -1;
            if (isFacingRight) Flip();
        }
        else if (transform.position.x <= -8f)
        {
            transform.position = new Vector3(-8f, transform.position.y, transform.position.z);
            moveDir = 1;
            if (!isFacingRight) Flip();
        }
    }

    protected virtual void ChangeStateType(StateType newState)
    {
        if (stateType == newState) return;

        stateType = newState;
    }


    IEnumerator FindPlayerRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.02f);

            targetDist = Vector3.Distance(transform.position, target.position);

            if (stateType == StateType.Idle || stateType == StateType.Move)
            {
                Vector3 monsterDir = Vector3.right * moveDir; // 몬스터가 바라보는 방향
                Vector3 playerDir = (transform.position - target.position).normalized; // 플레이어가 바라보는 방향

                float dotValue = Vector3.Dot(monsterDir, playerDir);
                isTrace = dotValue < 0f; // -1이므로 마주본 상태

                if (targetDist <= traceRange && isTrace)
                {
                    animator.SetBool("isRun", true);
                    isMove = true;
                    ChangeStateType(StateType.Trace);
                }
            }

            else if (stateType == StateType.Trace)
            {
                if (targetDist > traceRange)
                {
                    animator.SetBool("isRun", false);
                    ChangeStateType(StateType.Idle);
                }

                if (targetDist <= attackRange)
                {
                    ChangeStateType(StateType.Attack);
                }
            }
        }
    }
    #endregion

    void Idle()
    {
        isMove = false;

        timer += Time.deltaTime;
        if (timer >= idleTime)
        {
            isMove = true;
            timer = 0f;

            moveDir = Random.Range(0, 2) == 1 ? 1 : -1;
            if ((moveDir > 0 && !isFacingRight) || (moveDir < 0 && isFacingRight))
                Flip();

            walkTime = Random.Range(1f, 5f);
            animator.SetBool("isRun", true);
            ChangeStateType(StateType.Move);
        }
    }

    void Move()
    {
        moveVector = new Vector2(moveDir, 0);

        timer += Time.deltaTime;
        if (timer >= walkTime)
        {
            timer = 0f;
            idleTime = Random.Range(1f, 5f);
            animator.SetBool("isRun", false);
            ChangeStateType(StateType.Idle);
        }

        // 범위 안으로 들어오면 Trace로 전환
        if (targetDist <= traceRange)
        {
            ChangeStateType(StateType.Trace);
        }
    }

    void Trace()
    {
        isMove = true;
        animator.SetBool("isRun", true);
        Vector3 dirToPlayer = (target.position - transform.position).normalized;

        if (canFly)
        {
            moveVector = new Vector2(dirToPlayer.x, dirToPlayer.y);
            moveDir = dirToPlayer.x;
        }
        else
        {
            moveVector = new Vector2(moveDir, 0);
            moveDir = dirToPlayer.x > 0 ? 1 : -1;
        }

        if ((moveDir > 0 && !isFacingRight) || (moveDir < 0 && isFacingRight))
            Flip();
    }

    #region Hit
    public IEnumerator Hit(float damage)
    {
        if (isDead) yield break;

        isMove = false;
        animator.SetBool("isRun", false);
        SoundManager.Instance.PlaySound(sndHit); // Hit 사운드

        monsterHp -= damage;

        UIManager.Instance.SetHpEnemy(monsterHp, monsterMaxHp);

        if (monsterHp <= 0)
        {
            Death();
            yield break;
        }

        monsterRb.linearVelocity = Vector2.zero;
        animator.SetTrigger("Hit");
        
        yield return new WaitForSeconds(GetAnimLegnth("Hit") + 0.5f);

        ChangeStateType(StateType.Idle);
        isMove = true;
    }

    protected virtual void Death()
    {
        if (isDead) return;
        StartCoroutine(DeathRoutine());
    }

    protected IEnumerator DeathRoutine()
    {
        SoundManager.Instance.PlaySound(sndDie); // Die 사운드

        stateType = StateType.Death;
        isDead = true;

        animator.SetTrigger("Death");
        yield return new WaitForSeconds(GetAnimLegnth("Death") - 0.5f);

        if (CompareTag("Fly"))
            monsterRb.gravityScale = 1f;

        item.DropItem(transform.position);
        gameObject.SetActive(false);
        gameObject.layer = LayerMask.NameToLayer("DeadMonster");
    }


    #endregion

    #region Attack
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<KnightController>();

            if (!isPlayerDead)
                player.TakeDamage(AttackDamage());
        }
    }

    void Attack()
    {
        if (isAttacking || KnightController.isDead) return;

        StartCoroutine(AttackRoutine());
    }

    protected virtual IEnumerator AttackRoutine()
    {
        isAttacking = true;
        isMove = false;
        animator.SetBool("isRun", true);
        yield return null;

        monsterRb.linearVelocity = Vector2.zero;

        string randomAttack = attackAnimations[Random.Range(0, attackAnimations.Length)];
        animator.SetTrigger(randomAttack);
        yield return new WaitForSeconds(GetAnimLegnth(randomAttack));

        isAttacking = false;
        isMove = true;

        animator.SetBool("isRun", false);
        ChangeStateType(StateType.Idle);
    }

    public abstract float AttackDamage();
    #endregion

    float GetAnimLegnth(string stateName)
    {
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == stateName)
                return clip.length;
        }

        return 1f;
    }
}

