using UnityEngine;

public class HitBoxController : MonoBehaviour
{
    public KnightController knight;

    private void OnTriggerEnter2D(Collider2D other)
    {
        MonsterManager monster = other.GetComponent<MonsterManager>();

        if (monster != null && knight.isAttack)
        {
            StartCoroutine(monster.Hit(knight.defaultDamage));
        }
    }
}
