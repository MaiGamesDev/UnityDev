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
        bossDemon.isIdle = true;
        bossDemon.anim.SetBool("isWalk", false);
        bossDemon.anim.SetBool("isAttack", false);

        if (other.CompareTag("Player"))
        {
            bossDemon.anim.enabled = true;
            yield return new WaitForSeconds(2f);
            bossDemon.isIdle = false;

            bossDemon.bossState = BossDemon.BossState.Idle;
            gameObject.SetActive(false);
        }
    }
}
