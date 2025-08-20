using UnityEngine;

public class BossDemon : MonoBehaviour, IBossDefaultPattern
{
    public enum Demon { Sleep, Summon, IDLE, Walk}
    public Demon DemonState = Demon.IDLE;

    DemonSummon demonSummon;


    protected Animator animator;
    private Rigidbody2D slimeDemonRb;




    public float hp { get; set; }
    public float attackDamage { get; set; }
    public float moveSpeed { get; set; }

    protected virtual void Init(float hp, float attackDamage, float moveSpeed)
    {
       this.hp = hp;
       this.attackDamage = attackDamage;
       this.moveSpeed = moveSpeed;
    }
    private void Start()
    {
        slimeDemonRb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        demonSummon = GetComponent<DemonSummon>();

        DemonState = Demon.Sleep;
    }

    void Update()
    {
        switch (DemonState)
        {
            case Demon.Sleep:
                Sleep();
                break;
            case Demon.Summon:
                Summon();
                break;
            case Demon.IDLE:
                Idle();
                break;
            case Demon.Walk:
                Walk();
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            demonSummon.Summon();
        }
    }

    void Sleep()
    {

    }
    public void Summon()
    {
        animator.SetTrigger("Summon");
        gameObject.SetActive(false);

    }

    public void Idle()
    {
        animator.SetBool("is Walk", false);
    }

    public void DefaultAttack()
    {

    }

    public void Walk()
    {
        float x = Input.GetAxis("Horizontal");
        float moveHori = x * moveSpeed * Time.deltaTime;
        transform.position += new Vector3(moveHori, 0, 0);

        animator.SetBool("isWalk",true);
    }

    public void Death()
    {

    }
    public void Hit(float damage)
    {

    }
}
