using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BringerAttackStrategy : IBossAttackStrategy
{
    private GameObject spellPrefab;
    private float spellDamage;

    public BringerAttackStrategy(GameObject spellPrefab, float spellDamage)
    {
        this.spellPrefab = spellPrefab;
        this.spellDamage = spellDamage;
    }

    public void Execute(BossController boss)
    {
        int ranValue = Random.Range(0, 10);
        var animator = boss.GetComponent<Animator>();

        if (ranValue < 4)
        {
            Cast(boss);
        }
        else
        {
            animator.SetTrigger("Attack");
        }
    }

    private void Cast(BossController boss)
    {
        var animator = boss.GetComponent<Animator>();
        animator.SetTrigger("Cast");    
    }
}