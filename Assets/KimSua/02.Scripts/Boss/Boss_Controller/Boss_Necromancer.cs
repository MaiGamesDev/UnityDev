using System.Collections;
using UnityEngine;

public class Boss_Necromancer : BossController
{
    [SerializeField] private GameObject firePrefab;
    [SerializeField] private GameObject firePrefab2;
    [SerializeField] private GameObject evilWizard;
    [HideInInspector] public float fireDamage;

    protected override void SetupStrategies()
    {
        moveStrategy = new MoveStrategy();
        attackStrategy = new NecroAttackStrategy(firePrefab, firePrefab2);
    }

    protected override void SetupStats()
    {
        hp = 50f;
        attackDamage = 10f;
        fireDamage = 15f;
        moveSpeed = 2f;
        traceDist = 8f;
        attackDist = 6f;
        attackCooldown = 2f;
    }

    protected override void Start()
    {
        base.Start();

        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y, 0);
        Instantiate(evilWizard, spawnPos, Quaternion.identity);
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

    public void SpawnFire1()
    {
        StartCoroutine(SpawnFireRoutine());
    }

    IEnumerator SpawnFireRoutine()
    {        
        float interval = 0.5f;
        Vector3 basePos = target.position;

        for (int i = 0; i < 3; i++)
        {         
            Vector3 spawnPos = new Vector3(basePos.x + i * 1f, 2f + basePos.y, 0);
            GameObject fire = Instantiate(firePrefab, spawnPos, Quaternion.identity);

            fire.GetComponent<NecroFire>().Setup(fireDamage);

            Destroy(fire, 0.5f);

            yield return new WaitForSeconds(interval);
        }
    }

    public void SpawnFire2()
    {
        StartCoroutine(SpawnFireRoutine2());
    }

    IEnumerator SpawnFireRoutine2()
    {
        Vector3 spawnPos = new Vector3(target.position.x, -3.7f, 0f);
        GameObject fire2 = Instantiate(firePrefab2, spawnPos, Quaternion.identity);

        fire2.GetComponent<NecroFire>().Setup(fireDamage);

        yield return new WaitForSeconds(0.9f);

        Destroy(fire2);
    }
}
