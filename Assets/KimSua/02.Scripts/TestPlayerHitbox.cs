using UnityEngine;

public class TestPlayerHitbox : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<MonsterManager>() != null)
        {
            MonsterManager monster = other.GetComponent<MonsterManager>();
            StartCoroutine(monster.Hit(1));
        }
    }
}
