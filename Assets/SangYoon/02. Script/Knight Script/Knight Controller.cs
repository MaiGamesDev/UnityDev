
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;

public class KnightController : MonoBehaviour
{
    // Default Knight stats field
    //----------------------------------------------------------------------------------------
    // public float defaultHp = 2f; // GameManager.Instance.hp(싱글톤)으로 대체됨
    public float defaultDamage = 2f;
    public float hitboxAttackSpeed = 2.5f;
    public float animationAttackSpeed = 0.5f;
    //----------------------------------------------------------------------------------------

    public float monsterAttackDamage = 1f;

    public GameObject hitBox;

    private GameObject knightGb;
    private Animator animator;
    private Rigidbody2D knightRb;

    private Vector3 inputDir;

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpPower = 21f; // because the gravity value is 5.3

    public bool isGround;
    [SerializeField] public bool isAttack = true;
    [HideInInspector] public bool isHit; // Changed (06-27)

    public static bool isDead; // Changed (06-27)
    protected bool upgrade;

    //사운드 AudioCllip
    public AudioClip sndAttack;
    public AudioClip sndHurt;
    public AudioClip sndDie;
    public AudioClip sndJump;

    void Start()
    {
        knightRb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDead) return;

        InputKeyboard();
        SetAnimation();
        Jump();
        Attack();
    }

    private void FixedUpdate()
    {
        if (isDead) return;
        Run();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            animator.SetBool("isGround", true);
            isGround = true;
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
            SoundManager.Instance.PlaySound(sndJump); //Jump 사운드
            animator.SetTrigger("Jump");
            knightRb.AddForceY(jumpPower, ForceMode2D.Impulse);
        }
    }

    /// <summary>
    /// Attack (Use Z Key)
    /// </summary>
    public void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Z) && isAttack)
        {
            StartCoroutine(AttackCoroutine());
        }
    }

    // Changed (06-27)
    private IEnumerator AttackCoroutine()
    {
        isAttack = false;
        SoundManager.Instance.PlaySound(sndAttack); //Attack 사운드

        hitBox.SetActive(true);
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(hitboxAttackSpeed);

        hitBox.SetActive(false);
        isAttack = true;
    }

    /// <summary>
    /// Death motion
    /// </summary>
    void Death() // Changed (06-27)
    {
        if (GameManager.Instance.hp <= 0)
        {
            SoundManager.Instance.PlaySound(sndDie); //Death 사운드
            isDead = true;
            animator.SetTrigger("Death"); 
            knightRb.gravityScale = 1f;
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

    // Changed (06-27)
    public void TakeDamage(float damage) // this mathod using in MonsterHitBox OnTriggerEnter2D()
    {
        if (isDead) return;

        SoundManager.Instance.PlaySound(sndHurt); //hit 사운드

        UIManager.Instance.SetHp(GameManager.Instance.hp -= damage);

        if (!isHit)
        {
            animator.SetTrigger("Hit");
        }        

        if (GameManager.Instance.hp <= 0)
        {
            Death();
        }
    }
}
