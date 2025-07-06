using UnityEngine;

public class BossDemon : MonoBehaviour, IBossDefaultPattern
{
    public enum Demon { Summon, IDLE, Walk}
    public Demon slimeDemonState = Demon.IDLE;


    protected Animator animator;
    private Rigidbody2D slimeDemonRb;

    private void Start()
    {
        slimeDemonRb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public float hp { get; set; }
    public float attackDamage { get; set; }
    public float moveSpeed { get; set; }

    protected virtual void Init(float hp, float attackDamage, float moveSpeed)
    {
       this.hp = hp;
       this.attackDamage = attackDamage;
       this.moveSpeed = moveSpeed;
    }

    void Update()
    {
        switch (slimeDemonState)
        {
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

    public void Idle()
    {
        animator.SetBool("is Walk", false);
    }
    public void Death()
    {

    }

    public void DefaultAttack()
    {

    }

    public void Hit(float damage)
    {

    }


    public void Walk()
    {
        float x = Input.GetAxis("Horizontal");
        float moveHori = x * moveSpeed * Time.deltaTime;
        transform.position += new Vector3(moveHori, 0, 0);

        animator.SetBool("isWalk",true);
    }

    public void Summon()
    {
        DemonSummon demonSummon = GetComponent<DemonSummon>();
    }
}
