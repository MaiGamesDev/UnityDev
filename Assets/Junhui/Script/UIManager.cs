using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public float hp = 100f;
    public float maxHp = 100f;
    public int gold = 0;

    public RawImage Hp;
    public TextMeshProUGUI Gold;
    public RawImage Notice;
    public TextMeshProUGUI NoticeText;
    public RawImage HpEnemy;

    void Start()
    {
        ResetHp();
        SetGold(gold);
    }



    public void SetHp(float value)
    {
        // hp ����

        hp = value;
        if (hp >= 0)
        {
            Hp.GetComponent<RectTransform>().sizeDelta = new Vector2(hp * 4f, 18);
        }
        else if (hp <= 0)
        {
            Die();
        }
    }
    public void ResetHp()
    {
        // hp�� maxhp�� ����

        SetHp(maxHp);
    }
    public void SetGold(int value)
    {
        // gold ����

        gold = value;
        if (gold >= 0)
        {
            Gold.text = $"������ : {gold}";
        }
    }
    public void SetHpEnemy(float value, float maxValue)
    {
        //HpEnemy �ʺ� ����

        var result = value / maxValue * 100;
        HpEnemy.GetComponent<RectTransform>().sizeDelta = new Vector2(result * 5f, 18);
    }
    public IEnumerator ShowNotice(string value)
    {
        //�ȳ�â ǥ��

        StopCoroutine("ShowNotice");

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
