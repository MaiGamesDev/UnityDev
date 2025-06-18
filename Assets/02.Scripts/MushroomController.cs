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

    public float inter_MoveWaitTime; // 이동하기 전 대기시간
    private float current_interMWT;

    private Vector3 PlayerPos; // 플레이어의 좌표값

    private int randomInt; // 몬스터 랜덤 움직임
    private string direction;    

    private void RandomDirection()
   {
       Vector3.Set(0, 0, );
       randomInt = Random.Range(0, 2);
       switch (randomInt)
       {
           case 0:
               Vector3.x = -1f;

           case 1:
               Vector3.x = 0f;

           case 2:
               Vector3.x = 1f;

       } 
   }
    */
}
