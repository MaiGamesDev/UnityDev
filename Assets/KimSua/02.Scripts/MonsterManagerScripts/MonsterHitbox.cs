using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class MonsterHitbox : MonoBehaviour
{
    private MonsterManager owner;

    // ���� ��� (MonsterManager���� ȣ��)
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
                player.TakeDamage(owner.AttackDamage()); // ����� MonsterManager���� ���ݷ� ���
            }
        }
    }
}
