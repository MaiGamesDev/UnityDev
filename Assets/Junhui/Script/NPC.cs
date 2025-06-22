using TMPro;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public string idle;
    public string thankYou;
    public string noMoney;

    public UIManager uiManager;
    public TextMeshPro lineBox;

    public 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetLine(idle);
    }

    public void BuyItem(int price)
    {
        int gold = uiManager.gold;

        if (gold >= price)
        {
            uiManager.SetGold(gold - price);
            SetLine(thankYou);
        }
        else
            SetLine(noMoney);
    }

    void SetLine(string line)
    {
        Debug.Log(line);
        lineBox.text = line;
    }
}
