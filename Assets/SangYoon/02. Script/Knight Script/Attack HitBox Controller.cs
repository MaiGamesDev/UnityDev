using UnityEngine;

public class HitBoxController : MonoBehaviour
{
    public KnightController knight;
    MonsterManager monster;
    private IBossDefaultPattern bossPattern;

    private void OnTriggerEnter2D(Collider2D other)
    {
        monster = other.GetComponent<MonsterManager>();
        bossPattern = other.GetComponentInParent<IBossDefaultPattern>();

        if (knight.isAttack)
        {
            if (monster != null)
            {
                Debug.Log("몬스터 공격");
                StartCoroutine(monster.Hit(knight.defaultDamage));
            }
                

            if (bossPattern != null)
            {
                Debug.Log("보스 공격");
                bossPattern.Hit(knight.defaultDamage);
            }
            else
            {
                Debug.Log("보스를 찾지 못함");
            }
                
        }
    }
}
