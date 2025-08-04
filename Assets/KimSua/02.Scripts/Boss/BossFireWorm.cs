using UnityEngine;

public class BossFireWorm : MonoBehaviour, IBossDefaultPattern
{
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
        throw new System.NotImplementedException();
    }

    public void Walk()
    {
        throw new System.NotImplementedException();
    }

    public void Hit(float damage)
    {
        throw new System.NotImplementedException();
    }

    public void Death()
    {
        throw new System.NotImplementedException();
    }

    public void DefaultAttack()
    {
        throw new System.NotImplementedException();
    }
}
