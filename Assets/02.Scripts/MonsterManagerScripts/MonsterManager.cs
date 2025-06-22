using Middle_Age_2D_Game;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.DebugUI;

public abstract class MonsterManager : MonoBehaviour
{
    [SerializeField] protected float monsterHp = 10f;
    [SerializeField] protected float moveSpeed = 1f;
    protected Transform player;

    [SerializeField] protected float traceRange = 5f;
    [SerializeField] protected float attackRange = 1.5f;
    [SerializeField] protected float attackDamage = 3f;
    private bool isAttacking = false;
    private string[] attackAnimations = { "Attack", "Attack2" };
    [SerializeField] private GameObject attackHitbox;

    SpriteRenderer sRenderer;
    Animator animator;
    Rigidbody2D rb;

    private bool isMove = true;
    protected bool isTrackingPlayer = false;

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
        Move(); // 물리기반(MovePosition) 이동        
    }

    /// <summary>
    /// 플레이어가 맵에 들어왔을 때 호출하는 방법    
    /// </summary>
    /// public void OnPlayerEnterMap()
    /// {
    ///     monsterSpawner = FindFirstObjectByType<MonsterSpawner>();
    ///     monsterSpawner.SpawnMonster("Flying Eye", 2);
    ///     monsterSpawner.SpawnMonster("Goblin", 3);
    ///     monsterSpawner.SpawnMonster("Mushroom", 3);
    /// }


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

            if(moveRoutine == null)
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
        // flip 처리
        if (dir.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (dir.x > 0)
            transform.localScale = new Vector3(1, 1, 1);

        // 애니메이션 전환
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

    void MoveTo (Vector2 moveDir)
    {
        rb.MovePosition(rb.position + (moveDir * moveSpeed * Time.fixedDeltaTime));
    }


    void OnMouseDown()
    {
        StartCoroutine(Hit(1));
    }

    public IEnumerator Hit(float damage)
    {
        isMove = false;  

        animator.SetTrigger("Hit");

        monsterHp -= damage;

        if (monsterHp <= 0)
        {
            animator.SetTrigger("Death");

            item.DropItem(transform.position);

            yield return new WaitForSeconds(3f);

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

        yield return new WaitForSeconds(0.5f);
        isMove = true;
    }

    void Attack()
    {
        if (isAttacking) return; // 공격 중이라면 중복공격X
                    
        StartCoroutine(AttackRoutine());        
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        isMove = false;

        // 랜덤으로 애니메이션 선택
        string randomAttack = attackAnimations[Random.Range(0, attackAnimations.Length)];
        animator.SetTrigger(randomAttack);

        yield return new WaitForSeconds(GetAnimLegnth(randomAttack)); // 나머지 쿨타임
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

