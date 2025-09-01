using System.Collections;
using UnityEngine;

public class NecroAttackStrategy : IBossAttackStrategy
{
    [SerializeField] private GameObject firePrefab;
    private GameObject firePrefab2;

    public NecroAttackStrategy(GameObject firePrefab, GameObject firePrefab2)
    {
        this.firePrefab = firePrefab;
        this.firePrefab2 = firePrefab2;
    }

    public void Execute(BossController boss)
    {
        int ranValue = Random.Range(0, 10);
        var animator = boss.GetComponent<Animator>();
        

        if (ranValue < 5)
        {
            Fire1(boss);
        }
        else
        {
            Fire2(boss);
        }
    }

    private void Fire1(BossController boss)
    {
        var animator = boss.GetComponent<Animator>();
        animator.SetTrigger("Attack");
    }

    private void Fire2(BossController boss)
    {
        var animator = boss.GetComponent<Animator>();
        animator.SetTrigger("Attack2");
    }
}
