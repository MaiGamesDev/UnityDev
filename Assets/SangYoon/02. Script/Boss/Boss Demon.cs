using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BossDemon : MonoBehaviour
{
    public enum Demon { IDLE, Walk}
    public Demon DemonState = Demon.IDLE;

    public float hp = 500f;
    public float defaultAttackDamage = 10f;
    public float moveSpeed = 1.4f;
    public float skillDamage { get; private set; }

    public Animator anim;
    private Rigidbody2D DemonRb;
    private DemonSummon damonSummon;

    [SerializeField] private Transform target;

    protected virtual void Init(float hp, float attackDamage, float moveSpeed)
    {
       this.hp = hp;
       this.defaultAttackDamage = attackDamage;
       this.moveSpeed = moveSpeed;
    }

    private void Awake()
    {
        DemonRb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        damonSummon = GetComponent<DemonSummon>();

        anim.enabled = false;
    }
    private void Start()
    {
    }

    void Update()
    {
        switch (DemonState)
        {
            case Demon.IDLE:
                Idle();
                break;
            case Demon.Walk:
                Walk();
                break;
        }
    }


    public void Idle()
    {
        anim.SetTrigger("Idle");
    }

    public void Walk()
    {
        if (anim == null)
            return;

        Vector3 dir = (target.position - transform.position).normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;

        anim.SetBool("isWalk", true);
    }
}
