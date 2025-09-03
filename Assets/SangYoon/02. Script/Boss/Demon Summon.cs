using System.Collections;
using UnityEngine;

public class DemonSummon : MonoBehaviour
{
    public BossDemon bossDemon;


    private void Awake()
    {
        bossDemon = FindFirstObjectByType<BossDemon>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        StartCoroutine(SummonTriggerEnterRoutine(other));
    }
    private IEnumerator SummonTriggerEnterRoutine(Collider2D other)
    {
        bossDemon.isIdle = false;
        bossDemon.isWalk = false;
        bossDemon.isAttack = false;

        if (other.CompareTag("Player"))
        {
            bossDemon.anim.enabled = true;
            yield return new WaitForSeconds(2f);

            bossDemon.isIdle = true;
            bossDemon.bossState = BossDemon.BossState.Idle;
            gameObject.SetActive(false);
        }
    }
}
