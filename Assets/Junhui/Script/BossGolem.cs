using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System.Collections;

public class BossGolem : MonoBehaviour, IBossDefaultPattern
{
    public enum State { IDLE, WALK, ATTACK }
    public State state = State.IDLE;

    public float hp { get; set; } = 50f;
    public float attackDamage { get; set; } = 10f;
    public float moveSpeed { get; set; } = 0.7f;

    private float currHp;
    private float moveDir = 1;
    private bool isAttack = false;
    private bool isDead = false;

    private Transform target;
    private Animator animator;

void Start()
    {
        currHp = hp;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;
        switch (state)
        {
            case State.IDLE:
                Idle();
                break;
            case State.WALK:
                Walk();
                break;
            case State.ATTACK:
                DefaultAttack();
                break;
        }
    }

    public void Idle()
    {
        var targetDist = Vector3.Distance(transform.position, target.position);
        if (targetDist < 5)
        {
            ChangeState(State.ATTACK);
        }
        if (currHp <= 0)
        {
            Death();
        }
    }
    public void Walk()
    {
        transform.position += Vector3.right * moveSpeed * Time.deltaTime * moveDir; 

    }
    public void DefaultAttack()
    {
        if (!isAttack)
            StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        isAttack = true;
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.01f);
        float currAnimLength = animator.GetCurrentAnimatorStateInfo(0).length;
        Debug.Log(currAnimLength);
        yield return new WaitForSeconds(currAnimLength);
        isAttack = false;
        ChangeState(State.IDLE);
    }

    public void Hit(float damage)
    {

    }

    public void Death()
    {
        animator.SetTrigger("Death");
        isDead = true;
    }
    public void ChangeState(State newState)
    {
        if (state != newState)
            state = newState;
    }
}
