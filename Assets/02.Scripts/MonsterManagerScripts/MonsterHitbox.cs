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
        Debug.Log("히트박스 충돌 감지: " + other.name);

        if (owner == null)
        {
            Debug.LogError("owner가 설정되지 않았습니다!");
            return;
        }

        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<TestPlayerMoveAttack>();
            if (player != null)
            {
                Debug.Log("몬스터 공격력: " + owner.AttackDamage());
                player.TakeDamage(owner.AttackDamage()); // 몬스터의 공격력 사용
            }
        }
    }
}
