using UnityEngine;

public class BringerHitbox : MonoBehaviour
{
    public BossBringer bossBringer;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<KnightController>();

            player.TakeDamage(bossBringer.attackDamage);
        }
    }
}
