using System.Collections;
using UnityEngine;

public class Summon : MonoBehaviour
{
    #region 멤버변수
    public enum StateType { Spawn, Trace, Death }
    public StateType stateType;

    SpriteRenderer sRenderer;
    Animator animator;
    Rigidbody2D monsterRb;
    Collider2D monsterColl;
    ItemDropSpawner item;
    Transform target;

    protected bool canFly;
    private bool isFacingRight = false;
    protected bool isTrace;
    private bool isDead = false;
    bool isPlayerDead = false;

    private float minFlightHeight = -2.35f;
    [SerializeField] protected float moveSpeed = 1f;
    [SerializeField] private float traceRange = 5f;
    public float attackDamage = 3f;

    public float monsterHp = 10f;
    protected float monsterMaxHp;

    private float targetDist;

    private float moveDir;
    private Vector2 moveVector;

    [SerializeField] private AudioClip sndHit;
    [SerializeField] private AudioClip sndDie;
    #endregion
    // ----------------------------------------------------------------------------------------

    #region Default
    private void Awake()
    {
        sRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        monsterRb = GetComponent<Rigidbody2D>();
        monsterColl = GetComponent<Collider2D>();

        item = FindFirstObjectByType<ItemDropSpawner>();
        target = GameObject.FindGameObjectWithTag("Player").transform;

        canFly = gameObject.CompareTag("Fly");
    }

    void Start()
    {
        Spawn();
    }

    void Update()
    {
        isPlayerDead = KnightController.isDead;
        if (isPlayerDead) return;

        CheckBoundary();

        switch (stateType)
        {
            case StateType.Spawn:
                Spawn();
                break;
            case StateType.Trace:
                Trace();
                break;
            case StateType.Death:
                Death();
                break;
        }
    }

    public void Init(Transform player)
    {
        target = player;
        canFly = gameObject.CompareTag("Fly");
        monsterHp = 10f;
        monsterMaxHp = monsterHp;

        ChangeStateType(StateType.Trace);
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

    protected void ChangeStateType(StateType newState)
    {
        if (stateType == newState) return;

        stateType = newState;
    }


    #endregion

    void Spawn()
    {
        stateType = StateType.Spawn;
        animator.SetTrigger("Spawn");
    }

    void Trace()
    {
        stateType = StateType.Trace;

        Vector3 dirToPlayer = (target.position - transform.position).normalized;

        if (canFly)
        {
            moveVector = new Vector2(dirToPlayer.x, dirToPlayer.y);
            moveDir = dirToPlayer.x;
        }
        else
        {
            moveVector = new Vector2(moveDir, 0);
            moveDir = dirToPlayer.x > 0 ? 1 : -1;
        }

        if ((moveDir > 0 && !isFacingRight) || (moveDir < 0 && isFacingRight))
            Flip();
    }

    void Death()
    {
        if (isDead) return;

        stateType = StateType.Death;
        SoundManager.Instance.PlaySound(sndDie); // Die 사운드

        isDead = true;

        animator.SetTrigger("Death");

        if (CompareTag("Fly"))
            monsterRb.gravityScale = 1f;

        item.DropItem(transform.position);
        gameObject.SetActive(false);
        gameObject.layer = LayerMask.NameToLayer("DeadMonster");
    }

    // Attack
    // ----------------------------------------------------------------------------------------
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<KnightController>();

            if (!isPlayerDead)
                player.TakeDamage(attackDamage);
        }
    }
}
