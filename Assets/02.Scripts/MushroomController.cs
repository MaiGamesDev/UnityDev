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
    public int atk; // 공격력
    public float attackDelay; // 바로 공격x 딜레이 후 공격

   }
    */
}
