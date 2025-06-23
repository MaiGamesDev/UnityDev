using System.Collections;
using System.Reflection;
using UnityEngine;

namespace Middle_Age_2D_Game
{
    public class KnightController : MonoBehaviour, IMoveMethod
    {
        private float knightHP = 1;
        private float damage = 1;

        [SerializeField] protected float moveSpeed = 7f;
        [SerializeField] protected float jumpPower = 7f;

        private GameObject knightOb;
        private Rigidbody2D knightRb;
        private Animator knightAnim;
        private SpriteRenderer knightSR;

        private bool isGround = false;
        private float x; // X축 값을 넣기위한 변수
        //private SpriteRenderer spriteRd;

        public void Start()
        {
            knightOb = new GameObject();
            knightRb = GetComponent<Rigidbody2D>();
            knightAnim = GetComponent<Animator>();
            knightSR = GetComponent<SpriteRenderer>();
        }

        public void Update()
        {
            StartCoroutine(Run());
            Jump();
            Attack();
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

            if (isGround == true) // 땅에 닿아있을 때
            {
                if (x != 0) //움직일 때
                {
                    transform.localScale = (x > 0) ? new Vector3(1,1,1) : new Vector3(-1,1,1); // 캐릭터 방향, 콜라이더 함께 좌,우 방향제어
                    
                    knightAnim.SetBool("Run", true);
                    knightAnim.SetBool("Idle", false);

                }
                else //멈춰있을 때
                {
                    knightAnim.SetBool("Run", false);
                    knightAnim.SetBool("Idle", true);
                }
            }
            yield return null;
        }

        /// <summary>
        /// 점프하는 기능
        /// </summary>
        private void Jump()
        {
            if (Input.GetKeyDown(KeyCode.Space) && isGround == true)
            {
                knightRb.AddForce(transform.up * jumpPower, ForceMode2D.Impulse);
                knightAnim.SetTrigger("Jump");
            }
        }

        /// <summary>
        /// 공격 기능
        /// </summary>
        void Attack()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                knightAnim.SetTrigger("Attack");
            }
        }
    }
}
