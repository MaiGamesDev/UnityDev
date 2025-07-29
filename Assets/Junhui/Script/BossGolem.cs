using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System.Collections;
using UnityEngine.InputSystem.iOS;

public class BossGolem : MonoBehaviour, IBossDefaultPattern
{
    public AudioClip audioHit;
    public AudioClip audioDeath;
    public AudioClip audioAttack;

    public enum State { IDLE, WALK, ATTACK, RUSH }
    public State state = State.IDLE;

    public float hp { get; set; } = 50f;
    public float attackDamage { get; set; } = 10f;
    public float moveSpeed { get; set; } = 0.7f;

    private float currHp;
    private float moveDir = 1;
    private bool isAttack = false;
    private bool isDead = false;
    [SerializeField] private float rushSpeed;
    private bool isLeftRush = true;

    private Transform target;
    private Animator animator;

    void Start()
    {
        currHp = hp;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        ChangeState(State.RUSH);

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

    public void RushAttack()
    {
        animator.SetTrigger("Walk");
        if (isLeftRush)
            LeftRush();
        else
            RightRush();
    }

    public void LeftRush()
    {
        transform.localScale = new Vector3(-1,1,1);
        Vector2 pos = new Vector2(-6, -3);
        transform.position = Vector2.MoveTowards(transform.position, pos, Time.deltaTime * 12);
        if (transform.position.x == -6)
            isLeftRush = false;
    }
    public void RightRush()
    {
        transform.localScale = Vector3.one;
        Vector2 pos = new Vector2(6, -3);
        transform.position = Vector2.MoveTowards(transform.position, pos, Time.deltaTime * 12);
        if (transform.position.x == 6)
            isLeftRush = true;
    }

    IEnumerator AttackRoutine()
    {
        isAttack = true;
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.01f);
        float currAnimLength = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(currAnimLength);
        isAttack = false;
        ChangeState(State.IDLE);
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
        if (state != newState)
            state = newState;
    }

    public void PlayAttack()
    {
        SoundManager.Instance.PlaySound(audioAttack);
    }
}
