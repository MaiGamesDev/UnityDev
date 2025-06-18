using UnityEngine;

public class Enemy : MonoBehaviour
{
    float maxHp = 100f;
    float hp = 100f;
    public UIManager uiManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void Hurt(float value)
    {
        //적 체력 감소
        hp -= value;
        uiManager.SetHpEnemy(hp, maxHp);
    }
}
