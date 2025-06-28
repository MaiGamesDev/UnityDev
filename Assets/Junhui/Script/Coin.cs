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
        //사운드 재생, 코인 획득
        SoundManager.Instance.PlaySound(sndCoin);
        UIManager.Instance.StartCoroutine(UIManager.Instance.ShowNotice($"{value} 코인을 얻었다."));
        UIManager.Instance.SetGold(GameManager.Instance.gold + value);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            GetCoin();
    }
}
