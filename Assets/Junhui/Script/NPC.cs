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
        // ��尡 ���ݺ��� ������ ����
        int gold = GameManager.Instance.gold;

        if (gold >= price)
        {
            // type�� �ش��ϴ� ������ ����
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
        UIManager.Instance.StartCoroutine(UIManager.Instance.ShowNotice($"��⸦ �Ծ���. (ü�� +{value})"));
    }
    void BuyUpgrade()
    {
        float value = 1;
        GameManager.Instance.damage += value;
        UIManager.Instance.StartCoroutine(UIManager.Instance.ShowNotice($"���⸦ �����ߴ�. (���ݷ� +{value})"));
    }

    void SetLine(string line)
    {
        // ��� ����
        lineBox.text = line;
    }
}
