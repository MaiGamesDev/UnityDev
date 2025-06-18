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
        Reset_Hp();
        Set_Gold(gold);
        StartCoroutine(Show_Notice("안녕하세요"));
    }

    void Set_Hp(float value)
    {
        hp = value;
        if (hp > 0)
        {
            Hp.GetComponent<RectTransform>().sizeDelta = new Vector2(hp * 4f, 18);
        }
    }
    void Reset_Hp()
    {
        Set_Hp(maxHp);
    }
    void Set_Gold(int value)
    {
        gold = value;
        if (gold > 0)
        {
            Gold.text = $"소지금 : {gold}";
        }
    }
    void Set_HpEnemy(float value, float maxValue)
    {
        var result = value / maxValue * 100;
        HpEnemy.GetComponent<RectTransform>().sizeDelta = new Vector2(result * 5f, 18);
    }
    IEnumerator Show_Notice(string value)
    {
        NoticeText.text = value;
        Notice.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        Notice.gameObject.SetActive(false);

    }

}
