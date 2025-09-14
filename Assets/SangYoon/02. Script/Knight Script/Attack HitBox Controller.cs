using UnityEngine;

public class HitBoxController : MonoBehaviour
{
    public KnightController knight;
    MonsterManager monster;
    private IBossDefaultPattern bossPattern;

    private void OnTriggerEnter2D(Collider2D other)
    {
        monster = other.GetComponent<MonsterManager>();
        bossPattern = other.GetComponent<IBossDefaultPattern>();

        if (monster != null)
        {
            StartCoroutine(monster.Hit(knight.defaultDamage));
        }


        if (bossPattern != null)
        {
            bossPattern.Hit(knight.defaultDamage);
        }
    }
}
