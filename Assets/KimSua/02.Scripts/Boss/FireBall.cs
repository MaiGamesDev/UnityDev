using UnityEngine;

public class FireBall : MonoBehaviour
{
    private float damage;
    private Transform target;
    private float speed = 5f;
    private int moveDir = -1;

    private float minX = -9f;
    private float maxX = 9f;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        transform.Translate(Vector3.right * moveDir * speed * Time.deltaTime);

        // 화면 범위 벗어나면 삭제
        if (transform.position.x < minX || transform.position.x > maxX)
        {
            Destroy(gameObject);
        }
    }

    public void Attack(float fireDamage, int dir)
    {
        damage = fireDamage;
        moveDir = dir;

        Vector3 scale = transform.localScale;
        scale.x = dir > 0 ? 1 : -1;
        transform.localScale = scale;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<KnightController>();
            player.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
