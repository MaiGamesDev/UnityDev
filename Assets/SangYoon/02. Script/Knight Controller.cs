
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class KnightController : MonoBehaviour
{
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
        if (other.gameObject.CompareTag("Monster")) // hit Method
        {
            defaultHp -= monsterAttackDamage;
            Death();
            animator.SetTrigger("Hit");
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
        if (other.gameObject.CompareTag("Monster")) // hit Method
        {
            defaultHp -= monsterAttackDamage;
            Death();
        }
        animator.SetTrigger("Hit");
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
    void Death()
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





    void UpgradeStats()
    {

    }
}
