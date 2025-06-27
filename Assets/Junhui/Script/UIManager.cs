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

    void OnEnable()
    {
        SetGold(GameManager.Instance.gold);
        SetHp(GameManager.Instance.hp);
    }



    public void SetHp(float value)
    {
        // hp ����

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
        // hp�� maxhp�� ����

        SetHp(GameManager.Instance.maxHp);
    }
    public void SetGold(float value)
    {
        // gold ����

        GameManager.Instance.gold = value;
        if (value >= 0)
        {
            Gold.text = $"������ : {value} gold";
        }
    }
    public void SetHpEnemy(float value, float maxValue)
    {
        //HpEnemy �ʺ� ����
        HpEnemy.gameObject.SetActive(true);
        var result = value / maxValue * 100;
        HpEnemy.GetComponent<RectTransform>().sizeDelta = new Vector2(result * 5f, 18);
        if (value <= 0)
            HpEnemy.gameObject.SetActive(false);
    }

    public IEnumerator ShowNotice(string value)
    {
        //�ȳ�â ǥ��

        lastNotice = value;

        NoticeText.text = value;
        Notice.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        Notice.gameObject.SetActive(false);

    }
    public void Die()
    {
        // ���

        StartCoroutine(ShowNotice("�׾���ȴ�..."));

    }
}
