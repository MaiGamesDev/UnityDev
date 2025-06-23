using UnityEngine;

public class Enemy : MonoBehaviour
{
    float maxHp = 100f;
    float hp = 100f;

    void Start()
    {
        hp = maxHp;
    }


    public void Hurt(float value)
    {
        //적 체력 감소
        hp -= value;
        UIManager.Instance.SetHpEnemy(hp, maxHp);
    }
}
