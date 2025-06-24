using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using static UnityEditor.Progress;

public class TestPlayerMoveAttack : MonoBehaviour
{
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

        if (h == 0 && v == 0) // Idle ���� -> �������� �ʴ� ���� -> ��� Ű�� ������ ���� ����
        {
            //animator.SetBool("Run", false);
        }
        else // Run ���� -> �����̴� ���� -> ��� Ű �ϳ��� ���� ����
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
        //Debug.Log("�÷��̾� �ǰ�! ���� HP: " + hp);

        if (hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        //Debug.Log("�÷��̾� ���");
    }


}