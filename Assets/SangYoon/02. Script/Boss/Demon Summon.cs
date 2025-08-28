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
        SummonTriggerEnter(other);
    }
    private void SummonTriggerEnter(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            bossDemon.anim.enabled = true;
            gameObject.SetActive(false);
        }
    }
}
