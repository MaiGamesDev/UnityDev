using UnityEngine;

public class ExplosionTrigger : MonoBehaviour
{
    private FireExplosion parent;

    private void Start()
    {
        parent = GetComponentInParent<FireExplosion>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        parent?.Trigger(other);
    }
}
