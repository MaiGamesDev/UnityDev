using UnityEngine;

public class Coin : MonoBehaviour
{
    public AudioClip sndCoin;
    
    public int value = 10;

    void Start()
    {
    }

    void GetCoin()
    {
        //���� ���, ���� ȹ��
        SoundManager.Instance.PlaySound(sndCoin);
        UIManager.Instance.StartCoroutine(UIManager.Instance.ShowNotice($"{value} ������ �����."));
        UIManager.Instance.SetGold(GameManager.Instance.gold + value);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            GetCoin();
    }
}
