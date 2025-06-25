using UnityEngine;

public class HitBoxController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        int monsterLayer = LayerMask.NameToLayer("Monster");

        if (other.gameObject.layer == monsterLayer)
        {
        }
    }
}
