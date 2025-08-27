using UnityEngine;

public class DemonSummon : BossDemon
{
    public void Summon()
    {
        animator.SetTrigger("Summon");
        gameObject.SetActive(false);

    }
}
