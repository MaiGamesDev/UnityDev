using System.Collections;
using UnityEngine;

public class NecroFire : MonoBehaviour
{
    private float damage;
    private Transform target;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Setup(float fireDamage)
    {
        damage = fireDamage;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<KnightController>();
            player.TakeDamage(damage);
        }
    }

    public void DestroySpell()
    {
        Destroy(gameObject);
    }
}
