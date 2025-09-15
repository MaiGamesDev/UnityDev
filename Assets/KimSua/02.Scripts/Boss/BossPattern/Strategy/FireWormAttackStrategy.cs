using UnityEngine;

public class FireWormAttackStrategy : IBossAttackStrategy
{
    private GameObject firePrefab;
    private GameObject explosionPrefab;

    public FireWormAttackStrategy(GameObject firePrefab, GameObject explosionPrefab)
    {
        this.firePrefab = firePrefab;
        this.explosionPrefab = explosionPrefab;
    }

    public void Execute(BossController boss)
    {
        int ranValue = Random.Range(0, 10);
        var animator = boss.GetComponent<Animator>();
        animator.SetTrigger("Attack");

        if (ranValue < 6)
        {
            FireBallSpawn(boss);
        }
        else
        {
            FireExplosion(boss);
        }
    }

    private void FireBallSpawn(BossController boss)
    {
        int dir = (boss.target.position.x > boss.transform.position.x) ? 1 : -1;
        float spawnOffsetX = 3f * dir;
        Vector3 spawnPos = new Vector3(boss.transform.position.x + spawnOffsetX,
                                     boss.transform.position.y + 1f,
                                     boss.transform.position.z);

        GameObject fireball = Object.Instantiate(firePrefab, spawnPos, Quaternion.identity);
        var fireballScript = fireball.GetComponent<FireBall>();
        fireballScript?.Attack(boss.attackDamage, dir);
    }

    private void FireExplosion(BossController boss)
    {
        int count = Random.Range(1, 4);

        // 플레이어 위치에 하나 생성
        SpawnExplosion(boss, boss.target.position.x);

        // 나머지는 랜덤 위치에 생성
        for (int i = 1; i < count; i++)
        {
            float ranPosX = Random.Range(-7f, 7f);
            SpawnExplosion(boss, ranPosX);
        }
    }

    private void SpawnExplosion(BossController boss, float x)
    {
        Vector3 spawnPos = new Vector3(x, boss.transform.position.y, boss.transform.position.z);
        GameObject explosion = Object.Instantiate(explosionPrefab, spawnPos, Quaternion.identity);

        var explosionScript = explosion.GetComponent<FireExplosion>();
        explosionScript?.Attack(boss.attackDamage);
    }
}
