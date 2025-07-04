using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Controller : MonoBehaviour, IBossDefaultPattern
{
    public enum Boss1State { IDLE, WALK, TRACE, ATTACK }
    public Boss1State bossState;

    private Animator animator;
    private Rigidbody2D bosRb;
    private Collider2D bosColl;
    private ItemDropSpawner item;

    public Transform target;

    public float hp { get; set; }
    public float attackDamage { get; set; }
    public float moveSpeed { get; set; }
    private float moveDir;

    private float targetDist;

    public float currHp;
    private float timer;
    private float idleTime, walkTime;

    private bool isFacingRight = false;
    private bool isPlayerDead;
    private bool isDead;

    private bool isTrace;
    private bool isAttack;
    [SerializeField] private float traceDist = 8f;
    [SerializeField] private float attackDist = 3f;
    [SerializeField] private float attackTime = 2f;

    // --------------------------------------------------------------------

    void Awake()
    {
        animator = GetComponent<Animator>();
        bosRb = GetComponent<Rigidbody2D>();
        bosColl = GetComponent<Collider2D>();
        item = FindFirstObjectByType<ItemDropSpawner>();

        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Start()
    {
        hp = 50f;
        currHp = hp;
        attackDamage = 10f;
        moveSpeed = 1f;

        idleTime = Random.Range(1f, 5f);

        StartCoroutine(FindPlayerRoutine());
    }

    void Update()
    {
        if (isDead) return;

        isPlayerDead = KnightController.isDead;

        switch (bossState)
        {
            case Boss1State.IDLE:
                Idle();
                break;
            case Boss1State.WALK:
                Walk();
                break;
            case Boss1State.TRACE:
                Trace();
                break;
            case Boss1State.ATTACK:
                DefaultAttack();
                break;
        }

    }


    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // --------------------------------------------------------------------


    IEnumerator FindPlayerRoutine()
    {
        while (true)
        {
            yield return null;
            targetDist = Vector3.Distance(transform.position, target.position); // 플레이어와의 거리 계속 계산

            if (bossState == Boss1State.IDLE || bossState == Boss1State.WALK)
            {

                float dirToPlayer = transform.position.x - target.position.x; // 보스 기준 플레이어가 어느 쪽에 있는지

                //bool isPlayerInFront = (moveDir > 0 && dirToPlayer > 0) || (moveDir < 0 && dirToPlayer < 0);
                //// moveDir = 1 보스가 오른쪽 봄, dirToPlayer = 1 플레이어가 오른쪽에 있음

                //isTrace = isPlayerInFront;

                if (targetDist <= traceDist)
                {
                    animator.SetBool("isRun", true);
                    ChangeState(Boss1State.TRACE);
                }
            }

            else if (bossState == Boss1State.TRACE)
            {
                if (targetDist > traceDist)
                {
                    timer = 0f;
                    walkTime = Random.Range(1f, 5f);

                    animator.SetBool("isWalk", true);
                    ChangeState(Boss1State.WALK);
                }
            }

            if (targetDist < attackDist)
            {
                ChangeState(Boss1State.ATTACK);
            }
        }
    }

    public void Idle()
    {
        timer += Time.deltaTime;
        if (timer >= idleTime)
        {
            timer = 0f;

            moveDir = Random.Range(0, 2) == 1 ? 1 : -1;

            // 이동 방향과 현재 바라보는 방향이 다르면 Flip
            if ((moveDir > 0 && !isFacingRight) || (moveDir < 0 && isFacingRight))
                Flip();

            walkTime = Random.Range(1f, 5f);
            animator.SetBool("isWalk", true);
            ChangeState(Boss1State.WALK);
        }
    }

    public void Walk()
    {
        transform.position += Vector3.right * moveDir * moveSpeed * Time.deltaTime;

        timer += Time.deltaTime;
        if (timer >= walkTime)
        {
            timer = 0f;
            idleTime = Random.Range(1f, 5f);

            animator.SetBool("isWalk", false);
            ChangeState(Boss1State.IDLE);
        }
    }

    public void Trace() // 플레이어 발견
    {
        animator.SetBool("isWalk", true);
        var targetDir = (target.position - transform.position).normalized;
        transform.position += Vector3.right * targetDir.x * moveSpeed * Time.deltaTime;

        var direction = targetDir.x > 0 ? 1 : -1;

        if ((direction > 0 && !isFacingRight) || (direction < 0 && isFacingRight))
            Flip();

        moveDir = direction;
    }

    // Attack
    // ----------------------------------------------------------------------------------------

    public void DefaultAttack()
    {
        if (isAttack || KnightController.isDead) return;

        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        isAttack = true;
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(1f);

        animator.SetBool("isWalk", false);
        yield return new WaitForSeconds(attackTime - 0.5f);

        isAttack = false;
        animator.SetBool("isWalk", true);
        ChangeState(Boss1State.TRACE);

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<KnightController>();

            if (!isPlayerDead)
                player.TakeDamage(attackDamage);
        }
    }

    // Hit
    // ----------------------------------------------------------------------------------------

    public void Hit(float damage)
    {
        currHp -= damage;

        if (currHp <= 0f)
        {
            Death();
        }

        animator.SetTrigger("Hurt");
    }

    public void Death()
    {
        animator.SetTrigger("Death");
        bosColl.enabled = false;
        bosRb.gravityScale = 0f;

        item.DropItem(transform.position);
    }

    private void ChangeState(Boss1State newState)
    {
        if (bossState != newState)
            bossState = newState;
    }
}
