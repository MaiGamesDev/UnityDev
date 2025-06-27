using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class NPC : MonoBehaviour
{
    [SerializeField] private string idle;
    [SerializeField] private string thankYou;
    [SerializeField] private string noMoney;
    [SerializeField] private string type;

    public TextMeshPro lineBox;
    public GameObject item;
    public TextMeshPro priceText;
    public AudioClip sndItemBuy;
    public AudioClip sndNoMoney;

    [SerializeField] private float defaultPrice;
    [SerializeField] private float plusPrice;

    [SerializeField] private float plusValue;
    private float price = 0;


    void Start()
    {
        SetLine(idle);
        SetPrice();
    }

    public void BuyItem()
    {
        // ��尡 ���ݺ��� ������ ����
        float gold = GameManager.Instance.gold;

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
            SetPrice();
        }
        else
        { 
            SoundManager.Instance.PlaySound(sndNoMoney);
            SetLine(noMoney);
        }
    }

    void BuyMeat()
    {
        float value = plusValue;
        GameManager.Instance.maxHp += value;
        UIManager.Instance.ResetHp();
        UIManager.Instance.StartCoroutine(UIManager.Instance.ShowNotice($"��⸦ �Ծ���. (ü�� +{value})"));
    }
    void BuyUpgrade()
    {
        float value = plusValue;
        GameManager.Instance.damage += value;
        UIManager.Instance.StartCoroutine(UIManager.Instance.ShowNotice($"���⸦ �����ߴ�. (���ݷ� +{value})"));
    }

    void SetLine(string line)
    {
        // ��� ����
        lineBox.text = line;
    }

    void SetPrice()
    {
        float damage = Mathf.Clamp(GameManager.Instance.damage - 1,0,10000f) / plusValue;
        float maxHp = Mathf.Clamp(GameManager.Instance.maxHp - 100f, 0, 10000f) / plusValue;
        switch (type)
        {
            case "meat":
                price = defaultPrice + maxHp * plusPrice;
                Debug.Log(maxHp);
                break;
            case "upgrade":
                price = defaultPrice + damage * plusPrice;
                break;
            case "magic":
                price = defaultPrice;
                break;
        }
        priceText.text = $"{price}gold";

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            lineBox.gameObject.SetActive(true);
            item.SetActive(true);  
    }
    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
            lineBox.gameObject.SetActive(false);
            item.SetActive(false);
            SetLine(idle);
    }

}
