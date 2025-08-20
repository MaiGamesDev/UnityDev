using UnityEngine;

public class BossGolemHitbox : MonoBehaviour
{
    public BossGolem bossGolem;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<KnightController>();

            player.TakeDamage(bossGolem.attackDamage);
        }
    }
}
