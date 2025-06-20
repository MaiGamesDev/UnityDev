using Middle_Age_2D_Game;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.DebugUI;

public abstract class MonsterManager : SensingPlayer
{
    [SerializeField] protected float hp = 10f;
    [SerializeField] protected float moveSpeed = 1f;

    // [SerializeField] protected float attackRange = 1.3f;
    [SerializeField] protected float attackRange = 1f;

    [SerializeField] protected float attackDamage = 3f;
    private bool isAttacking = false;
    private string[] attackAnimations = { "Attack", "Attack2" };

    SpriteRenderer sRenderer;
    Animator animator;

    private bool isHit;

    private ItemDropSpawner item;

    public abstract void Init();

    private void Awake()
    {
        base.Awake();
        sRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        item = FindFirstObjectByType<ItemDropSpawner>();
        Init();
    }

    void Start()
    {
        Invoke("NextMove", 1f);
    }

    void Update()
    {
        Move();
        Attack();
        PlayerSensing();
    }

    void Move()
    {
        if (isHit)
            return;

        // 감지 중이면 랜덤이동 생략
        if (isTrackingPlayer)
        {
            animator.SetTrigger("Run");

            Vector3 direction = (player.transform.position - transform.position).normalized; // 목적지 - 현재 위치
            transform.position += direction * moveSpeed * Time.deltaTime;

            sRenderer.flipX = direction.x < 0; // 방향에 따라 flip

            animator.ResetTrigger("Idle");
            animator.SetTrigger("Run");
        }

        // 일반 랜덤 이동
        switch (stateType)
        {
            case StateType.Left:
                animator.SetTrigger("Run");
                transform.position += Vector3.left * moveSpeed * Time.deltaTime;
                sRenderer.flipX = true;
                break;

            case StateType.Right:
                animator.SetTrigger("Run");
                transform.position += Vector3.right * moveSpeed * Time.deltaTime;
                sRenderer.flipX = false;
                break;

            case StateType.Idle:
                animator.ResetTrigger("Run");
                animator.SetTrigger("Idle");
                break;
        }

        // 경계에 닿으면 방향 자동 반전
        if (transform.position.x > 8f)
            SetStateType(StateType.Left);
        else if (transform.position.x < -8f)
            SetStateType(StateType.Right);
    }

    void NextMove()
    {
        if (isTrackingPlayer) return;

        int rand = Random.Range(-1, 2); // -1, 0, 1
        StateType newState = rand == -1 ? StateType.Left :
                             rand == 0 ? StateType.Idle : StateType.Right;

        SetStateType(newState);

        Invoke("NextMove", 2f);
    }

    void OnMouseDown()
    {
        StartCoroutine(Hit(1));
    }

    IEnumerator Hit(float damage)
    {
        if (isHit)
            yield break;

        isHit = true;

        hp -= damage;

        if (hp <= 0)
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

        animator.SetTrigger("Hit");
        yield return new WaitForSeconds(0.5f);
        isHit = false;
    }

    void Attack()
    {
        if (isAttacking) return; // 공격 중이라면 중복공격X

        float distance = Vector2.Distance(player.transform.position, transform.position);

        if (player.transform.position.x <= attackRange)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;

        // 랜덤으로 애니메이션 선택
        string randomAttack = attackAnimations[Random.Range(0, attackAnimations.Length)];
        animator.SetTrigger(randomAttack);



        // player.GetComponent<IDamageable>().TakeDamage(attackDamage); // 플레이어에게 데미지 입힘
        // 만약 Attack의 경우 hitBox.SetActive(true);

        yield return new WaitForSeconds(GetAnimLegnth(randomAttack)); // 나머지 쿨타임
        isAttacking = false;
    }

    float GetAnimLegnth(string stateName)
    {
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == stateName)
                return clip.length;
        }

        return 1f;
    }

    /* 플레이어가 몬스터에 닿으면 플레이어 체력 -1
   void OnCollisionEnter2D(Collision2D other)
   {
       if (other.gameObject.CompareTag("Player"))
       {
           IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
           damageable.TakeDamage(attackDamage);
       }
   } 

    public void TakeDamage(float damage)
    {
        
    } */
}

