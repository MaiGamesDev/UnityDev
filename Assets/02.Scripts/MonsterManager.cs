using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public abstract class MonsterManager : MonoBehaviour
{
    [SerializeField] protected float hp = 10f;
    [SerializeField] protected float moveSpeed = 1f;

    // Rigidbody2D rigid;
    SpriteRenderer sRenderer;
    private enum StateType { Left, Stop, Right }
    private StateType stateType;

    public abstract void Init();

    private void Awake()
    {
        // rigid = GetComponent<Rigidbody2D>();

        sRenderer = GetComponent<SpriteRenderer>();
        Init();

    }

    private void Start()
    {
        Invoke("NextMove", 1f);
    }

    void Update()
    {
        Move();
    }

    void Move()
    {

        switch (stateType)
        {
            case StateType.Left:
                transform.position += Vector3.left * moveSpeed * Time.deltaTime;
                sRenderer.flipX = true;
                break;

            case StateType.Right:
                transform.position += Vector3.right * moveSpeed * Time.deltaTime;
                sRenderer.flipX = false;
                break;

            case StateType.Stop:
                // 정지
                break;
        }

        // 경계에 닿으면 방향 자동 반전
        if (transform.position.x > 8f)
            SetStateType(StateType.Left);
        else if (transform.position.x < -8f)
            SetStateType(StateType.Right);

        // 플레이어 방향으로 전진
        GameObject target = GameObject.Find("Player");
        Vector3 dir = target.transform.position - transform.position;
        dir.Normalize();

    }

    void SetStateType(StateType state)
    {
        stateType = state;
    }

    void NextMove()
    {
        int rand = Random.Range(-1, 2); // -1, 0, 1
        StateType newState = rand == -1 ? StateType.Left :
                             rand == 0 ? StateType.Stop : StateType.Right;

        SetStateType(newState);

        Invoke("NextMove", 5f);
    }


    private void OnTriggerEnter(Collider other)
    {

    }

    void OnMouseDown()
    {
        Hit(1);
    }

    void Hit(float damage) // 캐릭터가 공격할시로 변경 예정
    {
        hp -= damage;

        if (hp <= 0)
        {
            Destroy(gameObject);
            // 아이템 떨구는 기능 추가
        }
    }

    void Attack()
    {
        // hp -= player.attack; // 플레이어가 몬스터 공격했을 경우
        if (hp <= 0)
        {

        }

    }



}
