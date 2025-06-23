using UnityEngine;

public class Enemy : MonoBehaviour
{
    float maxHp = 100f;
    float hp = 100f;
    public UIManager uiManager;

    void Start()
    {
        hp = maxHp;
    }


    public void Hurt(float value)
    {
        //�� ü�� ����
        hp -= value;
        uiManager.SetHpEnemy(hp, maxHp);
    }
}
