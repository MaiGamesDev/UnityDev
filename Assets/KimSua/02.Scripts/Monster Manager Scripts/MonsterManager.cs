using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static BossBringer;
using static UnityEditor.PlayerSettings;

public abstract class MonsterManager : MonoBehaviour
{
    #region 멤버변수
    public enum StateType { Idle, Move, Trace, Attack }
    public StateType stateType;

    SpriteRenderer sRenderer;
    Animator animator;
    Rigidbody2D monsterRb;
    Collider2D monsterColl;

    protected bool canFly = false;
    private float minFlightHeight = -2.35f;

    [SerializeField] protected float moveSpeed = 1f;
    public float monsterHp = 10f;
    protected float monsterMaxHp;
    protected Transform target;
    bool isPlayerDead;

    private bool isFacingRight = false;
    private float idleTime, walkTime, targetDist;
    private float moveDir;
    private Vector2 move;
    private float timer;

    [SerializeField] private float traceRange = 5f;
    [SerializeField] private float attackRange = 2f;
    public float attackDamage = 3f;

    [HideInInspector] public bool isAttacking;
    private bool isDead = false;
    protected string[] attackAnimations = { "Attack" };
    [SerializeField] private GameObject attackHitbox;

    protected bool isMove;

    private ItemDropSpawner item;

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
        idleTime = Random.Range(1f, 5f);
        StartCoroutine(FindPlayerRoutine());
    }

    void Update()
    {
        isPlayerDead = KnightController.isDead;

        targetDist = Vector3.Distance(transform.position, target.position);

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
        }
    }

    private void FixedUpdate()
    {
        if (isDead) return;

        if (isMove)
        {
            Vector2 velocity = new Vector2(move.x * moveSpeed, move.y * moveSpeed);

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

    protected void ChangeStateType(StateType newState)
    {
        if (stateType != newState)
            stateType = newState;
    }

    IEnumerator FindPlayerRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            if (stateType == StateType.Idle || stateType == StateType.Move)
            {
                if (targetDist <= traceRange)
                {
                    animator.SetTrigger("Run");
                    ChangeStateType(StateType.Trace);
                }
            }

            else if (stateType == StateType.Trace)
            {
                if (targetDist > traceRange)
                {
                    timer = 0f;
                    walkTime = Random.Range(1f, 5f);

                    animator.SetTrigger("Run");
                    ChangeStateType(StateType.Move);
                }
            }

            if (targetDist < attackRange)
            {
                ChangeStateType(StateType.Attack);
            }
        }
    }
    #endregion

    void Idle()
    {
        isMove = false;
        move = Vector2.zero;

        timer += Time.deltaTime;
        if (timer >= idleTime)
        {
            timer = 0f;

            if (targetDist <= traceRange)
            {
                animator.SetTrigger("Run");
                ChangeStateType(StateType.Trace);
                return;
            }

            moveDir = Random.Range(0, 2) == 1 ? 1 : -1;

            if ((moveDir > 0 && !isFacingRight) || (moveDir < 0 && isFacingRight))
                Flip();

            walkTime = Random.Range(1f, 5f);
            animator.SetTrigger("Run");
            ChangeStateType(StateType.Move);
        }
    }

    void Move()
    {
        isMove = true;

        if (isMove)
        {
            move = new Vector2(moveDir, 0);

            timer += Time.deltaTime;
            if (timer >= walkTime)
            {
                timer = 0f;
                idleTime = Random.Range(1f, 5f);

                animator.SetTrigger("Idle");
                isMove = false;
                ChangeStateType(StateType.Idle);
            }
        }
        else
        {
            move = Vector2.zero;
        }
    }

    void Trace()
    {
        isMove = true;

        Vector3 dirToPlayer = (target.position - transform.position).normalized;

        if (canFly)
        {
            move = new Vector2(dirToPlayer.x, dirToPlayer.y);
            moveDir = dirToPlayer.x;
        }
        else
        {
            moveDir = dirToPlayer.x > 0 ? 1 : -1;
            move = new Vector2(moveDir, 0);
        }

        if ((moveDir > 0 && !isFacingRight) || (moveDir < 0 && isFacingRight))
            Flip();

        if (targetDist > traceRange)
        {
            animator.SetTrigger("Idle");
            ChangeStateType(StateType.Idle);
        }

        else if (targetDist < attackRange)
        {
            isMove = false;
            move = Vector2.zero;
            ChangeStateType(StateType.Attack);
        }
    }


    public IEnumerator Hit(float damage)
    {
        if (isDead)
            yield break;

        isMove = false;
        SoundManager.Instance.PlaySound(sndHit); // Hit 사운드

        monsterHp -= damage;


        UIManager.Instance.SetHpEnemy(monsterHp, monsterMaxHp);


        if (monsterHp <= 0)
            Death();

        animator.SetTrigger("Hit");
        yield return new WaitForSeconds(GetAnimLegnth("Hit"));

        isMove = true;
    }

    void Death()
    {
        SoundManager.Instance.PlaySound(sndDie); // Die 사운드

        isDead = true;

        animator.SetTrigger("Death");

        if (CompareTag("Fly"))
            monsterRb.gravityScale = 1f;

        item.DropItem(transform.position);
        gameObject.SetActive(false);
        gameObject.layer = LayerMask.NameToLayer("DeadMonster");
    }

    // Attack
    // ----------------------------------------------------------------------------------------
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

    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        isMove = false;

        string randomAttack = attackAnimations[Random.Range(0, attackAnimations.Length)];
        animator.SetTrigger(randomAttack);

        yield return new WaitForSeconds(GetAnimLegnth(randomAttack));

        isAttacking = false;
        isMove = true;

        yield return null;

        if (targetDist <= traceRange)
            ChangeStateType(StateType.Trace);
        else
            ChangeStateType(StateType.Idle);

    }

    public abstract float AttackDamage();

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

