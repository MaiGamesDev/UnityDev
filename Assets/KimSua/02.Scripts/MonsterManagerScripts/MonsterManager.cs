using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.DebugUI;

public abstract class MonsterManager : MonoBehaviour
{
    [SerializeField] protected float moveSpeed = 1f;
    public static float monsterHp = 10f; // Changed (06-25)
    protected Transform player;

    [SerializeField] private float traceRange = 5f, attackRange = 2f;
    public static float attackDamage = 3f; // Changed (06-25)

    private bool isAttacking, isTrackingPlayer, isDead = false;
    private string[] attackAnimations = { "Attack", "Attack2" };
    [SerializeField] private GameObject attackHitbox;

    SpriteRenderer sRenderer;
    Animator animator;
    Rigidbody2D rb;

    private bool isMove = true;

    protected enum StateType { Left, Idle, Right }
    protected StateType stateType;
    private Coroutine moveRoutine;

    private ItemDropSpawner item;

    public abstract void Init();

    private void Awake()
    {
        sRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        item = FindFirstObjectByType<ItemDropSpawner>();

        GameObject playerObj = GameObject.FindWithTag("Player");
        player = playerObj.transform;

        var hitbox = GetComponentInChildren<MonsterHitbox>();
        if (hitbox != null)
        {
            hitbox.SetOwner(this);
        }
    }

    void Start()
    {
        Init();

        Vector2 toPlayer = player.position - transform.position;

        if (toPlayer.x < 0)
        {
            SetStateType(StateType.Left);
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            SetStateType(StateType.Right);
            transform.localScale = new Vector3(1, 1, 1);
        }

        animator.ResetTrigger("Idle");
        animator.SetTrigger("Run");

        isTrackingPlayer = true;
    }


    void FixedUpdate()
    {
        if (isDead) return;

        Move();
    }

    protected void SetStateType(StateType state)
    {
        stateType = state;
    }


    void Move()
    {
        if (!isMove || player == null) return;

        float distance = Vector2.Distance(player.position, transform.position); // 플레이어와의 거리 비교
        Vector2 toPlayerDir = (player.position - transform.position).normalized; // 플레이어 방향으로 이동
        Vector2 ranMove = Vector2.zero; // 기본 랜덤이동

        if (distance <= traceRange)
        {
            // 추적 시작, 랜덤 이동 중지


            isTrackingPlayer = true;

            if (moveRoutine != null)
            {
                StopCoroutine(moveRoutine);
                moveRoutine = null;
            }

            ranMove = toPlayerDir;

            MoveAnimation(toPlayerDir);
            MoveTo(ranMove);

            if (distance <= attackRange)
            {
                Attack();
            }
        }

        else // 범위 밖 -> 랜덤이동
        {
            isTrackingPlayer = false;

            switch (stateType)
            {
                case StateType.Left:
                    ranMove = Vector2.left;
                    transform.localScale = new Vector3(-1, 1, 1);
                    break;
                case StateType.Right:
                    ranMove = Vector2.right;
                    transform.localScale = new Vector3(1, 1, 1);
                    break;
                case StateType.Idle:
                    ranMove = Vector2.zero;
                    break;
            }
            MoveAnimation(ranMove);
            MoveTo(ranMove);

            if (moveRoutine == null)
                moveRoutine = StartCoroutine(MoveRoutine());
        }
    }

    IEnumerator MoveRoutine()
    {
        while (!isTrackingPlayer)
        {
            int rand = Random.Range(-1, 2); // -1, 0, 1
            StateType newState = rand == -1 ? StateType.Left :
                                 rand == 0 ? StateType.Idle : StateType.Right;

            SetStateType(newState);

            yield return new WaitForSeconds(3f);
        }

        moveRoutine = null;
    }

    void MoveAnimation(Vector2 dir)
    {
        if (dir.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (dir.x > 0)
            transform.localScale = new Vector3(1, 1, 1);

        if (dir == Vector2.zero)
        {
            animator.ResetTrigger("Run");
            animator.SetTrigger("Idle");
        }
        else
        {
            animator.ResetTrigger("Idle");
            animator.SetTrigger("Run");
        }
    }

    void MoveTo(Vector2 moveDir)
    {
        Vector2 pos = rb.position + (moveDir * moveSpeed * Time.fixedDeltaTime);

        pos.x = Mathf.Clamp(pos.x, -8f, 8f);

        rb.MovePosition(pos);
    }


    public IEnumerator Hit(float damage)
    {
        if (isDead)
            yield break;

        isMove = false;
        monsterHp -= damage;
        rb.linearVelocity = Vector2.zero;

        if (monsterHp <= 0)
        {
            isDead = true;
            animator.SetTrigger("Death");

            foreach (Collider2D col in GetComponents<Collider2D>())
            {
                col.isTrigger = true;
            }
            rb.bodyType = RigidbodyType2D.Kinematic;
            yield return new WaitForSeconds(0.2f);

            item.DropItem(transform.position);
            yield return new WaitForSeconds(1.5f);

            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            Color c = sr.color;
            for (float t = 0; t < 1f; t += Time.deltaTime)
            {
                sr.color = new Color(c.r, c.g, c.b, 1f - t);
                yield return null;
            }
            gameObject.SetActive(false);
            yield break;
        }

        animator.SetTrigger("Hit");
        yield return new WaitForSeconds(GetAnimLegnth("Hit"));
        isMove = true;
    }

    void Attack()
    {
        if (isAttacking) return;
        isMove = false;

        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        isMove = false;
        rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;

        string randomAttack = attackAnimations[Random.Range(0, attackAnimations.Length)];
        animator.SetTrigger(randomAttack);


        yield return new WaitForSeconds(GetAnimLegnth(randomAttack));
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        isAttacking = false;
        isMove = true;
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

