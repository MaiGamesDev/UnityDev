using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class NPC : MonoBehaviour
{
    public string idle;
    public string thankYou;
    public string noMoney;
    public string type;

    public TextMeshPro lineBox;
    public AudioClip sndItemBuy;
    public AudioClip sndNoMoney;


    void Start()
    {
        SetLine(idle);
    }

    public void BuyItem(int price)
    {
        // 골드가 가격보다 많을시 구매
        int gold = GameManager.Instance.gold;

        if (gold >= price)
        {
            // type에 해당하는 아이템 구매
            switch (type)
            {
                case "meat":
                    BuyMeat();
                    break;
                case "upgrade":
                    BuyUpgrade(); 
                    break;
            }


            SoundManager.Instance.PlaySound(sndItemBuy);
            UIManager.Instance.SetGold(gold - price);
            SetLine(thankYou);
        }
        else
        { 
            SoundManager.Instance.PlaySound(sndNoMoney);
            SetLine(noMoney);
        }
    }

    void BuyMeat()
    {
        float value = 10;
        GameManager.Instance.maxHp += value;
        UIManager.Instance.ResetHp();
        UIManager.Instance.StartCoroutine(UIManager.Instance.ShowNotice($"고기를 먹었다. (체력 +{value})"));
    }
    void BuyUpgrade()
    {
        float value = 1;
        GameManager.Instance.damage += value;
        UIManager.Instance.StartCoroutine(UIManager.Instance.ShowNotice($"무기를 제련했다. (공격력 +{value})"));
    }

    void SetLine(string line)
    {
        // 대사 설정
        lineBox.text = line;
    }
}
