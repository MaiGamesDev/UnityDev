using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    private float currentDamage;

    public void EnableAttack(float damage)
    {
        this.currentDamage = damage;
        gameObject.SetActive(true);
    }

    public void DisableAttack()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<KnightController>();
        if (player != null)
        {
            if (other.CompareTag("Player"))
            {
                player.TakeDamage(currentDamage);
            }
        }
    }
}
