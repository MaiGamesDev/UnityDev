using UnityEngine;

public class DemonSummon : BossDemon
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetTrigger("Summon");
            gameObject.SetActive(false);
        }
    }
}
