using System.Collections;
using UnityEngine;

public class FireExplosion : MonoBehaviour
{
    private float damage;
    private Transform target;
    [SerializeField] private GameObject[] explosions;
    private bool isAttack = false;
    private bool isBlinking = false;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

        foreach (var obj in explosions)
        {
            obj.SetActive(false);
        }
    }

    public void Attack(float damage)
    {
        if (!isBlinking && !isAttack)
            StartCoroutine(BlinkingRoutine(damage));
    }


    IEnumerator BlinkingRoutine(float fireDamage)
    {
        isBlinking = true;
        this.damage = fireDamage;

        float blinkSpeed = 6f;
        float blinkDuration = 1f;
        float timer = 0f;
        bool visible = true;

        while (timer < blinkDuration)
        {
            explosions[0].SetActive(visible);
            visible = !visible;
            timer += 1f / blinkSpeed;
            yield return new WaitForSeconds(1f / blinkSpeed);
        }

        explosions[0].SetActive(false);

        isAttack = true;
        isBlinking = false;
        
        if (!isAttack) yield break;
        if (explosions.Length > 1)
        {
            explosions[1].SetActive(true);
        }

        Destroy(gameObject, 0.5f);
    }

    public void Trigger(Collider2D other)
    {
        if (!isAttack) return;

        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<KnightController>();
            player.TakeDamage(damage);
        }
    }
}
