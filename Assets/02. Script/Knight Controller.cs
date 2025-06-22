using System.Collections;
using System.Reflection;
using UnityEngine;

namespace Middle_Age_2D_Game
{
    public class KnightController : MonoBehaviour, IMoveMethod
    {
        [SerializeField] protected float moveSpeed = 7f;
        [SerializeField] protected float jumpPower = 7f;

        private GameObject knightOb;
        private Rigidbody2D knightRb;
        private Animator knightAnim;

        public bool isGround = false;
        private float x; // X축 값을 넣기위한 변수
        //private SpriteRenderer spriteRd;

        public void Start()
        {
            knightOb = new GameObject();
            knightRb = GetComponent<Rigidbody2D>();
            knightAnim = GetComponent<Animator>();
        }

        public void Update()
        {
            StartCoroutine(Run());
            Jump();
        }

        void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.CompareTag("Ground"))
            {
                isGround = true;
            }
        }

        void OnCollisionExit2D(Collision2D other)
        {
            if (other.collider.CompareTag("Ground"))
            {
                isGround = false;
            }
        }

        /// <summary>
        /// 좌우만 움직이는 기능
        /// </summary>
        private IEnumerator Run()
        {
            
            x = Input.GetAxis("Horizontal"); // X축 이동
            transform.position += new Vector3(x, 0, 0) * moveSpeed * Time.deltaTime;

            if (x > 0)
            {
                knightAnim.SetBool("Run", true);
                knightAnim.SetBool("Idle", false);
                yield return null;
            }
            else
            {
                knightAnim.SetBool("Run", false);
                knightAnim.SetBool("Idle", true);
            }
        }

        /// <summary>
        /// 점프하는 기능
        /// </summary>
        private void Jump()
        {
            if (Input.GetKeyDown(KeyCode.Space) && isGround == true)
            {
                 knightRb.AddForce(transform.up * jumpPower, ForceMode2D.Impulse);
            }
        }

        /// <summary>
        /// 공격 기능
        /// </summary>
        void Attack()
        {

        }
    }
}
