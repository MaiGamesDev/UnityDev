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
    bool isPlayerDead = false;

    private bool isFacingRight = false;
    private float idleTime, walkTime, targetDist;
    private float moveDir;
    private Vector2 move;
    private float timer;

    [SerializeField] private float traceRange = 5f;
    [SerializeField] private float attackRange = 2f;
    public float attackDamage = 3f;

    [HideInInspector] public bool isAttacking;

    protected string[] attackAnimations = { "Attack" };
    [SerializeField] private GameObject attackHitbox;

    protected bool isMove = true;
    protected bool isHit = false;
    private bool isDead = false;

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
        if (isPlayerDead) return;

        targetDist = Vector3.Distance(transform.position, target.position);

        CheckBoundary(); // x범 위 제한

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

    protected void ChangeStateType(StateType newState)
    {
        if (stateType == newState) return;

        stateType = newState;

        /*
        switch (stateType)
        {
            case StateType.Idle:
                animator.SetTrigger("Idle");
                break;
            case StateType.Move:
            case StateType.Trace:
                animator.SetTrigger("Run");
                break;
            case StateType.Attack:
                break;
        } */
    }

    IEnumerator FindPlayerRoutine()
    {
        WaitForSeconds delay = new WaitForSeconds(0.2f);

        while (true)
        {
            if (target != null)
            {
                float targetDist = Vector2.Distance(transform.position, target.position);

                if (stateType == StateType.Idle || stateType == StateType.Move && targetDist <= traceRange)
                {
                    ChangeStateType(StateType.Trace);
                }

                else if (stateType == StateType.Trace && targetDist > traceRange)
                {
                    ChangeStateType(StateType.Move);
                }

                if (targetDist < attackRange && stateType != StateType.Attack)
                {
                    ChangeStateType(StateType.Attack);
                }

                yield return delay;
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
            isMove = true;

            if (targetDist <= traceRange)
            {
                // animator.SetTrigger("Run");
                ChangeStateType(StateType.Trace);
                return;
            }

            moveDir = Random.Range(0, 2) == 1 ? 1 : -1;

            if ((moveDir > 0 && !isFacingRight) || (moveDir < 0 && isFacingRight))
                Flip();

            walkTime = Random.Range(1f, 5f);
            // animator.SetTrigger("Run");
            ChangeStateType(StateType.Move);
        }
    }

    void Move()
    {
        if (isMove && stateType != StateType.Idle)
        {
            move = new Vector2(moveDir, 0);

            timer += Time.deltaTime;
            if (timer >= walkTime)
            {
                timer = 0f;
                idleTime = Random.Range(1f, 5f);

                // animator.SetTrigger("Idle");
                ChangeStateType(StateType.Idle);
            }
        }
    }

    void Trace()
    {
        /*
        isMove = true;
        animator.SetTrigger("Run");

        if (targetDist > traceRange)
        {
            timer = 0f;
            animator.SetTrigger("Idle");
            ChangeStateType(StateType.Idle);
        }

        else if (targetDist < attackRange)
        {
            ChangeStateType(StateType.Attack);
        } */

        Vector3 dirToPlayer = (target.position - transform.position).normalized;
        transform.position += Vector3.right * dirToPlayer.x * moveSpeed * Time.deltaTime;

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
    }


    public IEnumerator Hit(float damage)
    {
        if (isDead || isHit)
            yield break;

        isHit = true;
        isMove = false;
        SoundManager.Instance.PlaySound(sndHit); // Hit 사운드

        monsterHp -= damage;
        move = Vector2.zero;

        UIManager.Instance.SetHpEnemy(monsterHp, monsterMaxHp);


        if (monsterHp <= 0)
        {
            Death();
            yield break;
        }

        animator.SetTrigger("Hit");
        yield return new WaitForSeconds(GetAnimLegnth("Hit"));

        animator.SetTrigger("Idle");
        yield return new WaitForSeconds(0.5f);

        ChangeStateType(StateType.Idle);
        isHit = false;
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

    protected virtual IEnumerator AttackRoutine()
    {
        isAttacking = true;
        isMove = false;

        move = Vector2.zero;
        string randomAttack = attackAnimations[Random.Range(0, attackAnimations.Length)];
        animator.SetTrigger(randomAttack);

        yield return new WaitForSeconds(GetAnimLegnth(randomAttack));

        isAttacking = false;
        isMove = true;

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

