using System.Collections;
using UnityEngine;

public class Boss_Fireworm : BossController
{
    [SerializeField] private GameObject firePrefab;
    [SerializeField] private GameObject exGroup;
    private bool canDamage = false;

    protected override void SetupStrategies()
    {
        moveStrategy = new MoveStrategy();
        attackStrategy = new FireWormAttackStrategy(firePrefab, exGroup);
    }

    protected override void SetupStats()
    {
        hp = 40f;
        attackDamage = 15f;
        moveSpeed = 1f;
        traceDist = 10f;
        attackDist = 8f;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player") && !canDamage)
        {
            var player = other.collider.GetComponent<KnightController>();
            if (!KnightController.isDead)
            {
                player.TakeDamage(attackDamage);
                StartCoroutine(Knockback(player));
            }
        }
    }

    private IEnumerator Knockback(KnightController player)
    {
        canDamage = true;

        Vector2 knockbackDir = (player.transform.position - transform.position).normalized;
        player.GetComponent<Rigidbody2D>().AddForce(knockbackDir * 10f, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.5f);

        canDamage = false;
    }
}
