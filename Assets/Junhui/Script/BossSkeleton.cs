using UnityEngine;

public class BossSkeleton : MonoBehaviour, IBossDefaultPattern
{
    public AudioClip audioHit;
    public AudioClip audioDeath;
    public AudioClip audioAttack;
    public enum State { IDLE, WALK, ATTACK }
    public State state = State.IDLE;

    public float hp { get; set; } = 30f;
    public float attackDamage { get; set; } = 10f;
    public float moveSpeed { get; set; } = 0.7f;

    private float currHp;
    private float moveDir = 1;
    private bool isWalk = false;
    private bool isAttack = false;
    private bool isDead = false;
    private bool isIdle = false;

    private Transform target;
    private Animator animator;

    void Start()
    {
        currHp = hp;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }
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
        if (!isIdle)
        {
            animator.SetTrigger("Idle");
            isIdle = true;
        }

        var targetDist = Vector3.Distance(transform.position, target.position);
        if (targetDist < 3)
        {
            ChangeState(State.ATTACK);
        }
        if (targetDist >= 3)
        {
            ChangeState(State.WALK);
        }
        if (currHp <= 0)
        {
            Death();
        }
    }

    public void Walk()
    {
        if (!isWalk)
        {
            isWalk = true;
            animator.SetTrigger("Walk");
        }
        if (target.position.x > transform.position.x)
        {
            moveDir = 1;
            transform.localScale = new Vector3(-1,1,1);
        }
        else
        {
            moveDir = -1;
            transform.localScale = new Vector3(1, 1, 1);
        }
            transform.position += Vector3.right * moveSpeed * Time.deltaTime * moveDir;
        var targetDist = Vector3.Distance(transform.position, target.position);
        if (targetDist < 3)
        {
            ChangeState(State.ATTACK);
        }
    }

    public void DefaultAttack()
    {
        if (!isAttack)
        {
            isAttack = true;
            animator.SetTrigger("Attack");
        }
        var targetDist = Vector3.Distance(transform.position, target.position);
        if (targetDist >= 3)
        {
            ChangeState(State.WALK);
        }
    }

    public void Hit(float damage)
    {
        SoundManager.Instance.PlaySound(audioHit);
        animator.SetTrigger("Hurt");
        currHp -= damage;
        if (currHp <= 0)
        {
            Death();
        }
   
    }
    public void Death()
    {
    
        SoundManager.Instance.PlaySound(audioDeath);
        animator.SetTrigger("Death");
        isDead = true;
    }
    public void ChangeState(State newState)
    {
        isAttack = false;
        isIdle = false;
        isWalk = false;
        if (state != newState)
            state = newState;
    }
}
