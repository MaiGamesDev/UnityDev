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
        // hp 설정

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
        // hp를 maxhp로 리셋

        SetHp(maxHp);
    }
    public void SetGold(int value)
    {
        // gold 설정

        gold = value;
        if (gold >= 0)
        {
            Gold.text = $"소지금 : {gold}";
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

        StopCoroutine("ShowNotice");

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
