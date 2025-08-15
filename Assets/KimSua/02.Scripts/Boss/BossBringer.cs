using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBringer : MonoBehaviour, IBossDefaultPattern
{
    #region 멤버변수
    public enum Boss1State { IDLE, WALK, TRACE, ATTACK, CAST, HIT }
    public Boss1State bossState;

    private Animator animator;
    private Rigidbody2D bossRb;
    private Collider2D bossColl;
    private ItemDropSpawner item;

    [SerializeField] private GameObject spellPrefab;

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

    private bool isAttack;
    private bool isMove = true;

    [HideInInspector] public float spellDamage;
    [SerializeField] private float traceDist = 8f;
    [SerializeField] private float attackDist = 3f;
    private float attackCooldown = 3f;
    private float lastAttackTime;

    [SerializeField] private AudioClip sndHit;
    [SerializeField] private AudioClip sndDie;
    #endregion

    // --------------------------------------------------------------------

    void Awake()
    {
        animator = GetComponent<Animator>();
        bossRb = GetComponent<Rigidbody2D>();
        bossColl = GetComponent<Collider2D>();
        item = FindFirstObjectByType<ItemDropSpawner>();

        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Start()
    {
        hp = 50f;
        currHp = hp;
        attackDamage = 10f;
        spellDamage = 15f;
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
            case Boss1State.HIT:
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
                // float dirToPlayer = transform.position.x - target.position.x; // 보스 기준 플레이어가 어느 쪽에 있는지

                if (targetDist <= traceDist)
                {
                    animator.SetBool("isWalk", true);
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

            // 공격범위, 쿨다운 체크
            if (targetDist < attackDist && bossState != Boss1State.ATTACK && Time.time - lastAttackTime >= attackCooldown)
            {
                lastAttackTime = Time.time;
                ChangeState(Boss1State.ATTACK);
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

            if (targetDist <= traceDist)
            {
                animator.SetBool("isWalk", true);
                ChangeState(Boss1State.TRACE);
                return;
            }

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
        if (isMove)
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
    }

    public void Trace() // 플레이어 발견
    {
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
        isMove = false;
        isAttack = true;
        animator.SetBool("isWalk", false);
        yield return null;

        int ranValue = Random.Range(0, 10);

        if (ranValue < 4)
        {
            ChangeState(Boss1State.CAST);
            Cast();
        }

        else
        {
            animator.SetTrigger("Attack");
        }

        bossRb.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(1f);
        isAttack = false;
        isMove = true;

        ChangeState(Boss1State.IDLE);        
    }

    public void Cast()
    {
        animator.SetTrigger("Cast");

        Vector3 spawnPos = new Vector3(target.position.x, -3f, 0f);
        GameObject spell = Instantiate(spellPrefab, spawnPos, Quaternion.identity);

        var spellScript = spell.GetComponent<BringerSpell>();
        if (spellScript != null)
        {
            spellScript.Setup(spellDamage);
        }
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
        isMove = false;
        SoundManager.Instance.PlaySound(sndHit); // Hit 사운드

        currHp -= damage;

        animator.SetBool("isWalk", false);
        bossRb.linearVelocity = Vector2.zero;

        UIManager.Instance.SetHpEnemy(currHp, hp);

        if (currHp <= 0)
        {
            Death();
            return;
        }            

        ChangeState(Boss1State.HIT);
        StartCoroutine(HitRoutine());
    }

    private IEnumerator HitRoutine()
    {
        animator.SetTrigger("Hurt");
        yield return new WaitForSeconds(1f);
        isMove = true;
        ChangeState(Boss1State.IDLE);
    }

    public void Death()
    {
        isDead = true;

        SoundManager.Instance.PlaySound(sndDie); // Die 사운드

        animator.SetTrigger("Death");
        bossColl.enabled = false;
        bossRb.gravityScale = 0f;

        item.DropItem(transform.position);

        Invoke("DisableSelf", 1f); // 1초 뒤 실행        
    }

    private void DisableSelf()
    {
        gameObject.SetActive(false);
    }

    private void ChangeState(Boss1State newState)
    {
        if (bossState != newState)
            bossState = newState;
    }
}
