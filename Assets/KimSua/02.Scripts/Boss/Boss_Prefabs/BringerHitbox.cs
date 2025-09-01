using UnityEngine;

public class BringerHitbox : MonoBehaviour
{
    [SerializeField] private Boss_Bringer bossBringer;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<KnightController>();

            player.TakeDamage(bossBringer.attackDamage);

            if (player.isHit) return;
        }
    }
}
