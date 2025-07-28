using UnityEngine;

public class BringerSpell : MonoBehaviour
{
    public GameObject spell;
    private Transform target;
    private BossBringer bossBringer;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        bossBringer = GetComponent<BossBringer>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<KnightController>();

            player.TakeDamage(bossBringer.spellDamage);
        }
    }

    public void DestroySpell()
    {
        Destroy(gameObject);
    }
}
