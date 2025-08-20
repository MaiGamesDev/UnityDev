using System.Collections;
using UnityEngine;
using static BossBringer;
using static UnityEditor.Progress;
using static UnityEngine.GraphicsBuffer;

public class BossFireWorm : MonoBehaviour, IBossDefaultPattern
{
    #region 멤버변수
    public enum BossState { Idle, Walk, Trace, Attack, Hit, Death }
    public BossState bossState;

    public float hp { get; set; }
    public float attackDamage { get; set; }
    public float moveSpeed { get; set; }

    Animator anim;
    Rigidbody2D fireWormRb;
    Collider2D fireWormColl;
    ItemDropSpawner item;
    Transform target;
    private bool isFacingRight;

    private float timer;

    private float moveDir;
    private float targetDist;

    private float currHp;

    private float idleTime, walkTime;
    [SerializeField] private float traceDist = 10f;
    [SerializeField] private float attackDist = 7f;

    private bool isAttack;
    private bool isMove = true;

    [SerializeField] private GameObject firePrefab;

    #endregion

    #region Default Setting
    private void Awake()
    {
        anim = GetComponent<Animator>();
        fireWormRb = GetComponent<Rigidbody2D>();
        fireWormColl = GetComponent<Collider2D>();
        item = FindFirstObjectByType<ItemDropSpawner>();

        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Start()
    {
        hp = 40f;
        currHp = hp;
        attackDamage = 15f;
        moveSpeed = 1f;

        idleTime = Random.Range(1f, 5f);

        StartCoroutine(FindPlayerRoutine());
    }

    void Update()
    {
        CheckBoundary(); // x범위 제한

        switch (bossState)
        {
            case BossState.Idle:
                Idle();
                break;
            case BossState.Walk:
                Walk();
                break;
            case BossState.Trace:
                Trace();
                break;
            case BossState.Attack:
                DefaultAttack();
                break;
            case BossState.Hit:
                break;
            case BossState.Death:
                break;
        }

        targetDist = Vector3.Distance(transform.position, target.position);
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

    private void ChangeState(BossState newState)
    {
        if (bossState != newState)
            bossState = newState;
    }
    #endregion

    IEnumerator FindPlayerRoutine()
    {
        while (true)
        {
            yield return null;
            targetDist = Vector3.Distance(transform.position, target.position); // 플레이어와의 거리 계속 계산

            if ((bossState == BossState.Idle || bossState == BossState.Walk))
            {
                if (targetDist <= traceDist)
                {
                    anim.SetBool("isWalk", true);
                    ChangeState(BossState.Trace);
                }
            }

            else if (bossState == BossState.Trace)
            {
                if (targetDist > traceDist)
                {
                    timer = 0f;
                    walkTime = Random.Range(1f, 5f);

                    anim.SetBool("isWalk", true);
                    ChangeState(BossState.Walk);
                }
            }

            if (targetDist < attackDist && bossState != BossState.Attack)
            {
                if (bossState != BossState.Attack)
                    anim.SetBool("isWalk", true);

                StartCoroutine(AttackRoutine());
            }
        }
    }

    public void Idle()
    {
        isMove = false;

        timer += Time.deltaTime;
        if (timer >= idleTime)
        {
            timer = 0f;
            isMove = true;
            anim.SetBool("isWalk", true);
            moveDir = Random.Range(0, 2) == 1 ? 1 : -1;

            if ((moveDir > 0 && !isFacingRight) || (moveDir < 0 && isFacingRight))
                Flip();

            if (targetDist <= traceDist)
            {
                ChangeState(BossState.Trace);
                return;
            }

            walkTime = Random.Range(1f, 5f);
            ChangeState(BossState.Walk);
        }
    }

    public void Walk()
    {
        if (!isMove) return;

        transform.position += Vector3.right * moveDir * moveSpeed * Time.deltaTime;

        timer += Time.deltaTime;
        if (timer >= walkTime)
        {
            timer = 0f;
            idleTime = Random.Range(1f, 5f);

            anim.SetBool("isWalk", false);
            ChangeState(BossState.Idle);
        }
    }

    public void Trace()
    {
        Vector3 dir = (target.position - transform.position).normalized;
        transform.position += Vector3.right * dir.x * moveSpeed * Time.deltaTime;

        var direction = dir.x > 0 ? 1 : -1;

        if ((direction > 0 && !isFacingRight) || (direction < 0 && isFacingRight))
            Flip();

        if (targetDist <= attackDist)
        {
            ChangeState(BossState.Attack);
            return;
        }
    }

    #region Hit
    public void Hit(float damage)
    {
        if (bossState == BossState.Hit || bossState == BossState.Death) return;

        currHp -= damage;

        if (currHp > 0)
        {
            anim.SetTrigger("Hit");
            ChangeState(BossState.Hit);
            StartCoroutine(HitRoutine());
        }

        else
        {
            anim.SetTrigger("Death");
            ChangeState(BossState.Death);
            Death();
        }
    }

    private IEnumerator HitRoutine()
    {
        yield return new WaitForSeconds(1f);
        ChangeState(BossState.Walk);
        Debug.Log("상태 전환 : Hit -> Move");
    }

    public void Death()
    {
        anim.SetTrigger("Death");
        fireWormColl.enabled = false;
        item.DropItem(transform.position);
        Invoke("DisableSelf", 1f);
    }

    private void DisableSelf()
    {
        gameObject.SetActive(false);
    }
    #endregion

    #region Attack
    public void DefaultAttack()
    {
        if (KnightController.isDead) return;

        if (bossState != BossState.Attack)
            StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        isAttack = true;
        isMove = false;
        anim.SetBool("isWalk", false);
        yield return null;

        anim.SetTrigger("Attack");
        
        FireBallSpawn();
        yield return new WaitForSeconds(3f);

        fireWormRb.linearVelocity = Vector2.zero;

        isAttack = false;
        isMove = true;

        ChangeState(BossState.Idle);
    }

    public void FireBallSpawn()
    {
        Vector3 spawnPos = new Vector3(transform.position.x, -1.6f, 0f);
        GameObject fireball = Instantiate(firePrefab, spawnPos, Quaternion.identity);

        var fireballScript = fireball.GetComponent<FireBall>();
        if (fireballScript != null)
        {
            int dir = (target.position.x > transform.position.x) ? 1 : -1;
            fireballScript.Attack(attackDamage, dir);
        }
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
        {
            var player = other.collider.GetComponent<KnightController>();

            if (KnightController.isDead)
                player.TakeDamage(attackDamage);
        }
    }
    #endregion


}
