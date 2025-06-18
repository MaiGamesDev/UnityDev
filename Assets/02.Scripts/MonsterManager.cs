using Middle_Age_2D_Game;
using System.Collections;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.DebugUI;

public abstract class MonsterManager : MonoBehaviour
{
    [SerializeField] protected float hp = 10f;
    [SerializeField] protected float moveSpeed = 1f;


    [SerializeField] protected float attackRange = 1f;
    [SerializeField] protected float attackDamage = 3f;
    private bool isAttacking = false;

    SpriteRenderer sRenderer;
    Animator animator;
    GameObject player;

    private bool isHit;

    private enum StateType { Left, Idle, Right }
    private StateType stateType;

    public abstract void Init();

    private void Awake()
    {
        sRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");

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
    }

    void Move()
    {
        if (isHit)
            return;

        animator.SetTrigger("Run");

        switch (stateType)
        {
            case StateType.Left:
                transform.position += Vector3.left * moveSpeed * Time.deltaTime;
                sRenderer.flipX = true;
                break;

            case StateType.Right:
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


        /* 플레이어 감지시 플레이어 방향으로 이동(구현 중)

        if (gameObject.CompareTag("Player"))
        {
            GameObject target = GameObject.Find("Player");
            dir = target.transform.position - transform.position;
            dir.Normalize();
        } */
    }

    void SetStateType(StateType state)
    {
        stateType = state;
    }

    void NextMove()
    {
        int rand = Random.Range(-1, 2); // -1, 0, 1
        StateType newState = rand == -1 ? StateType.Left :
                             rand == 0 ? StateType.Idle : StateType.Right;

        SetStateType(newState);

        Invoke("NextMove", 3f);
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

        float distance = Vector2.Distance(player.transform.position, transform.position); // 플레이어와의 거리 파악
        if (distance <= attackRange)
        {
            StartCoroutine(AttackRoutine());            
        }
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");

        float attackDuration = GetAnimLegnth("Attack");

        yield return new WaitForSeconds(attackDuration * 0.5f);
        // player.GetComponent<IDamageable>().TakeDamage(attackDamage); // 데미지 입힘

        yield return new WaitForSeconds(attackDuration * 0.5f); // 나머지 쿨타임
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
}
