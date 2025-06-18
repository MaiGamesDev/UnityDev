using System.Reflection;
using UnityEngine;

namespace Middle_Age_2D_Game
{
    public class KnightController : MonoBehaviour, IMoveMethod
    {
        [SerializeField] protected float moveSpeed = 7f;
        [SerializeField] protected float jumpPower = 7f;

        private GameObject gameObj;
        private Rigidbody2D knightRb;
        //private SpriteRenderer spriteRd;

        public void Start()
        {
            gameObj = new GameObject();
            knightRb = gameObject.AddComponent<Rigidbody2D>();
        }

        public void Update()
        {
            Move();
            Jump();
        }

        /// <summary>
        /// 좌우만 움직이는 기능
        /// </summary>
        private void Move()
        {
            float x = Input.GetAxis("Horizontal"); // X축 이동
            transform.position += new Vector3(x, 0, 0) * moveSpeed * Time.deltaTime;
        }
        
        /// <summary>
        /// 점프하는 기능
        /// </summary>
        private void Jump()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                knightRb.AddForce(transform.up * jumpPower, ForceMode2D.Impulse);
            }
        }
    }
}
