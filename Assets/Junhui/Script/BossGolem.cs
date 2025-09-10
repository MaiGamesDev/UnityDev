using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System.Collections;
using UnityEngine.InputSystem.iOS;

public class BossGolem : MonoBehaviour, IBossDefaultPattern
{
    public AudioClip audioHit;
    public AudioClip audioDeath;
    public AudioClip audioAttack;
    public AudioClip audioRush;

    public enum State { IDLE, WALK, ATTACK, RUSH }
    public State state = State.IDLE;

    public float hp { get; set; } = 50f;
    public float attackDamage { get; set; } = 10f;
    public float moveSpeed { get; set; } = 0.7f;

    private float currHp;
    private float moveDir = 1;
    private bool isAttack = false;
    private bool isDead = false;
    [SerializeField] private float rushSpeed = 12;
    private bool isLeftRush = true;
    private bool isRush = false;
    private bool isIdle = false;
    private int attackCount = 0;
    
    public GameObject rushCollider;

    private Transform target;
    private Animator animator;

    void Start()
    {
        transform.position = new Vector2(6,-3);

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
            case State.RUSH:
                RushAttack();
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
        if (targetDist < 5)
        {
            ChangeState(State.ATTACK);
        }
        if (targetDist > 12)
        {
            ChangeState(State.RUSH);
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
        yield return new WaitForSeconds(currAnimLength);
        isAttack = false;

        attackCount++;
        if (attackCount > 1)
        {
            ChangeState(State.RUSH);
            attackCount = 0;
        }
        else
            ChangeState(State.IDLE);
    }

    public void RushAttack()
    {
        if (!isRush)
        {
            animator.SetTrigger("Walk");
            rushCollider.SetActive(true);
            SoundManager.Instance.PlaySound(audioRush);
            isRush = true;
        }
        if (isLeftRush)
            LeftRush();
        else
            RightRush();
    }

    public void LeftRush()
    {
        Debug.Log("left");
        transform.localScale = new Vector3(-1, 1, 1);
        Vector2 pos = new Vector2(-6, -3);
        transform.position = Vector2.MoveTowards(transform.position, pos, Time.deltaTime * rushSpeed);
        if (transform.position.x == -6)
        {
            ChangeState(State.IDLE);
            transform.localScale = Vector3.one;
            isLeftRush = false;
            rushCollider.SetActive(false);
        }
    }
    public void RightRush()
    {
        transform.localScale = Vector3.one;
        Vector2 pos = new Vector2(6, -3);
        transform.position = Vector2.MoveTowards(transform.position, pos, Time.deltaTime * rushSpeed);
        if (transform.position.x == 6)
        {
            ChangeState(State.IDLE);
            transform.localScale = new Vector3(-1, 1, 1);
            isLeftRush = true;
            rushCollider.SetActive(false);
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
        isRush = false;
        isIdle = false;
        if (state != newState)
            state = newState;
    }

    public void PlayAttack()
    {
        SoundManager.Instance.PlaySound(audioAttack);
    }
}
