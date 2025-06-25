using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using static UnityEditor.Progress;

public class TestPlayerMoveAttack : MonoBehaviour
{
    /*
    //private Animator animator;
    [SerializeField] private float moveSpeed = 3f;
    private float h, v;
    private bool isAttack = false;
    public float hp = 3f;

    void Start()
    {
        //animator = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
        Attack();
    }

    void Move()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        if (h == 0 && v == 0) // Idle 상태 -> 움직이지 않는 상태 -> 어떠한 키도 누르지 않은 상태
        {
            //animator.SetBool("Run", false);
        }
        else // Run 상태 -> 움직이는 상태 -> 어떠한 키 하나라도 누른 상태
        {
            int scaleX = h > 0 ? 1 : -1;
            transform.localScale = new Vector3(scaleX, 1, 1);

            //animator.SetBool("Run", true);

            var dir = new Vector3(h, v, 0).normalized;
            transform.position += dir * moveSpeed * Time.deltaTime;
        }
    }

    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isAttack)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    IEnumerator AttackRoutine()
    {
        isAttack = true;


        yield return new WaitForSeconds(0.25f);
        isAttack = false;
    }

  
    //void OnCollisionEnter2D(Collision2D other)
    //{
    //    if (other.gameObject.GetComponent<IItem>() != null)
    //    {
    //        IItem item = other.gameObject.GetComponent<IItem>();
    //        item.Get();
    //    }
    //}


    public void TakeDamage(float damage)
    {
        hp -= damage;
        //Debug.Log("플레이어 피격! 남은 HP: " + hp);

        if (hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        //Debug.Log("플레이어 사망");
    } */

    // Default Knight stats field
    //----------------------------------------------------------------------------------------
    public float defaultHp = 2f;
    public float defaultDamage = 2f;
    public float defaultAttackSpeed = 1f;
    //----------------------------------------------------------------------------------------

    // For this field use when Knight stats upgrade
    //-----------------------------------------------------------------------------------------
    private float upDamage; // 승수에 따라 몬스터 체력 구현이 뒷받침 되어야함
    private float upAttackSpeed; // 승수는 0.1 ~ 0.03으로 구현해야함 (기본 공격 속도가 1이기 때문)
    private float upHp;
    //-----------------------------------------------------------------------------------------

    public float monsterAttackDamage = 1f;

    public GameObject hitBox;

    private GameObject knightGb;
    private Animator animator;
    private Rigidbody2D knightRb;

    private Vector3 inputDir;

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpPower = 21f; // because the gravity value is 5.3

    private bool isGround;
    private bool isAttack;

    void Start()
    {
        knightRb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

    }

    void Update()
    {
        InputKeyboard();
        SetAnimation();
        Jump();
        StartCoroutine(Attack());
    }

    private void FixedUpdate()
    {
        Run();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            animator.SetBool("isGround", true);
            isGround = true;
        }
        //if (other.gameObject.CompareTag("Monster")) // hit Method
        //{
        //    defaultHp -= monsterAttackDamage;
        //    Death();
        //}
        //animator.SetTrigger("Hit");
    }

    public void TakeDamage(float damage)
    {
        animator.SetTrigger("Hit");
        defaultHp -= damage;
        Debug.Log("플레이어 피격! 남은 HP: " + defaultHp);

        if (defaultHp <= 0)
        {
            Death();
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            animator.SetBool("isGround", false);
            isGround = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<MonsterManager>() != null) // hit Method
        {
            MonsterManager monster = other.GetComponent<MonsterManager>();
            StartCoroutine(monster.Hit(defaultAttackSpeed));
        }

    }
    /// <summary>
    /// 키보드 입력
    /// </summary>
    void InputKeyboard()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        inputDir = new Vector3(x, y, 0);
    }

    /// <summary>
    /// 움직이는 기능
    /// </summary>
    void Run()
    {
        if (inputDir.x != 0)
            knightRb.linearVelocityX = inputDir.x * moveSpeed;
    }

    /// <summary>
    /// Jump (Gravity : 5.3)
    /// </summary>
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            animator.SetTrigger("Jump");
            knightRb.AddForceY(jumpPower, ForceMode2D.Impulse);
        }
    }

    /// <summary>
    /// Attack (Use Z Key)
    /// </summary>
    IEnumerator Attack()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !isAttack)
        {
            isAttack = true;
            hitBox.SetActive(true);
            animator.SetTrigger("Attack");

            yield return new WaitForSeconds(defaultAttackSpeed);
            hitBox.SetActive(false);
            isAttack = false;

            MonsterManager.monsterHp -= defaultAttackSpeed;

            Debug.Log("공격했음");
        }
    }


    /// <summary>
    /// Death motion
    /// </summary>
    public void Death()
    {
        if (defaultHp <= 0)
        {
            animator.SetTrigger("Death");
            defaultHp -= MonsterManager.attackDamage;
        }
    }

    /// <summary>
    /// Run motion in Animator
    /// </summary>
    void SetAnimation()
    {
        if (inputDir.x != 0)
        {
            animator.SetBool("isRun", true);

            var scaleX = inputDir.x > 0 ? 1 : -1;
            transform.localScale = new Vector3(scaleX, 1, 1);
        }
        else
        {
            animator.SetBool("isRun", false);
        }
    }
}


