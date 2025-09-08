using UnityEngine;

public class Boss_Bringer : BossController
{
    [SerializeField] private GameObject spellPrefab;
    [HideInInspector] public float spellDamage;

    protected override void SetupStrategies()
    {
        moveStrategy = new MoveStrategy();
        attackStrategy = new BringerAttackStrategy(spellPrefab, spellDamage);
    }

    protected override void SetupStats()
    {
        hp = 50f;
        attackDamage = 10f;
        spellDamage = 15f;
        moveSpeed = 1f;
        traceDist = 8f;
        attackDist = 5f;
        attackCooldown = 2f;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<KnightController>();
            if (!KnightController.isDead)
                player.TakeDamage(attackDamage);
        }
    }

    public void SpawnSpell()
    {
        Vector3 spawnPos = new Vector3(target.position.x, -3f, 0f);
        GameObject spell = Instantiate(spellPrefab, spawnPos, Quaternion.identity);

        var spellScript = spell.GetComponent<BringerSpell>();
        spellScript?.Setup(spellDamage);
    }
}
