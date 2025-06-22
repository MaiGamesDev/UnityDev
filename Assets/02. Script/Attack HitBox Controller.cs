using UnityEngine;

public class AttackHitBoxController : MonoBehaviour
{
    public GameObject[] attackFrames; // 공격 프레임별 콜라이더 실행을 위한 배열

    public void EnableHitBox(int index)
    {
        DisableHitBox();
        if (index >= 0 && index < attackFrames.Length)
        {
            attackFrames[index].SetActive(true);
        }
    }

    public void DisableHitBox()
    {
        foreach (var attackFrame in attackFrames)
        {
            attackFrame.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        int monsterLayer = LayerMask.NameToLayer("Monster");

        if (other.gameObject.layer == monsterLayer)
        {
            //몬스터 레이어 오브젝트가 가진 체력에서 데미지 들어가도록 구현 필요 (서로의 변수값을 몰라서 그냥 두었음)
        }
    }
}
