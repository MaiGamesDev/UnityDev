using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class MonsterHitbox : MonoBehaviour
{
    public MonsterManager monster;

    private void OnTriggerEnter2D(Collider2D other)
    {
        KnightController knight = other.GetComponent<KnightController>();

        if (knight != null && monster.isAttacking && !KnightController.isDead)
        {
            knight.TakeDamage(monster.AttackDamage()); // abstract method use
        }
    }
}
