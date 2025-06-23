using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class MonsterHitbox : MonoBehaviour
{
    private MonsterManager owner;

    // 몬스터 등록 (MonsterManager에서 호출)
    public void SetOwner(MonsterManager monster)
    {
        owner = monster;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<TestPlayerMoveAttack>();
            if (player != null)
            {
                Debug.Log("몬스터 공격력: " + owner.AttackDamage());
                player.TakeDamage(owner.AttackDamage()); // 연결된 MonsterManager에서 공격력 사용
            }
        }
    }
}
