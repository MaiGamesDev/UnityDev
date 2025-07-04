using UnityEngine;

public class BossSlimeDemon : MonoBehaviour, IBossDefaultPattern
{
    public enum SlimeDemon { IDLE, PATROL, TRACE, ATTACK, HIT, DEATH, SKILLS}
    public SlimeDemon slimeDemonState = SlimeDemon.IDLE;


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
            case SlimeDemon.IDLE:
                Idle();
                break;
            case SlimeDemon.PATROL:
                break;
            case SlimeDemon.TRACE:
                break;
            case SlimeDemon.ATTACK:
                break;
            case SlimeDemon.HIT:
                break;
            case SlimeDemon.DEATH:
                break;
            case SlimeDemon.SKILLS:
                break;

        }
    }

    public void Idle()
    {

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

    }





    private Rigidbody2D demonRb;
    private Animator animator;

    private void Start()
    {
        demonRb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
}
