using UnityEngine;

public class BossFireWorm : MonoBehaviour, IBossDefaultPattern
{
    public enum Boss1State { Idle, Walk, Trace, Attack, Hit, Death }
    public Boss1State bossState;

    public float hp { get; set; }
    public float attackDamage { get; set; }
    public float moveSpeed { get; set; }

    

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Idle()
    {
        
    }

    public void Walk()
    {
        
    }

    public void Hit(float damage)
    {
        
    }

    public void Death()
    {
        
    }

    public void DefaultAttack()
    {
        
    }
}
