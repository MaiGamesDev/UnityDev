using UnityEngine;

public interface IBossDefaultPattern
{
    public float hp { get; set; }
    public float attackDamage { get; set; }
    public float moveSpeed { get; set; }

   
    
    public void Idle();

    public void Walk();

    public void Hit();

    public void Death();

    public void DefaultAttack();

}
