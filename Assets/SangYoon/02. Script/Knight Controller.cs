using UnityEngine;

public class KnightController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D knightRb;

    private Vector3 inputDir;

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpPower = 9f;

    public bool isGround;

    void Start()
    {
        knightRb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        InputKeyboard();

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

        SetAnimation();
        Jump();
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
    /// 점프기능
    /// </summary>
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround == true)
        {
            animator.SetTrigger("Jump");
            knightRb.AddForceY(jumpPower, ForceMode2D.Impulse);
        }
    }

    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {

        }
    }
    /// <summary>
    /// 애니메이터 상의 애니메이션 모아둠
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
