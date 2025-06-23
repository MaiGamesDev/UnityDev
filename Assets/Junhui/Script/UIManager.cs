using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;


    public static UIManager Instance
    {
        get
        {
            if (instance == null) instance = new UIManager();
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public RawImage Hp;
    public TextMeshProUGUI Gold;
    public RawImage Notice;
    public TextMeshProUGUI NoticeText;
    public RawImage HpEnemy;

    private string lastNotice = "";

    void Start()
    {
        ResetHp();
        SetGold(GameManager.Instance.gold);
        SetHp(GameManager.Instance.hp);
    }



    public void SetHp(float value)
    {
        // hp 설정

        GameManager.Instance.hp = value;
        if (value >= 0)
        {
            Hp.GetComponent<RectTransform>().sizeDelta = new Vector2(value * 4f, 18);
        }
        else if (GameManager.Instance.hp <= 0)
        {
            Die();
        }
    }
    public void ResetHp()
    {
        // hp를 maxhp로 리셋

        SetHp(GameManager.Instance.maxHp);
    }
    public void SetGold(int value)
    {
        // gold 설정

        GameManager.Instance.gold = value;
        if (value >= 0)
        {
            Gold.text = $"소지금 : {value} gold";
        }
    }
    public void SetHpEnemy(float value, float maxValue)
    {
        //HpEnemy 너비 설정

        var result = value / maxValue * 100;
        HpEnemy.GetComponent<RectTransform>().sizeDelta = new Vector2(result * 5f, 18);
    }

    public IEnumerator ShowNotice(string value)
    {
        //안내창 표시

        lastNotice = value;

        NoticeText.text = value;
        Notice.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        Notice.gameObject.SetActive(false);

    }
    public void Die()
    {
        // 사망

        StartCoroutine(ShowNotice("죽어버렸다..."));

    }
}
