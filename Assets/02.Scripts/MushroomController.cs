using JetBrains.Annotations;
using UnityEngine;

public class MushroomController : MonsterManager
{
    public override void Init()
    {
        hp = 5f;
        moveSpeed = 3f;
    }

    /*
    public int atk; // ���ݷ�
    public float attackDelay; // �ٷ� ����x ������ �� ����

   }
    */
}
