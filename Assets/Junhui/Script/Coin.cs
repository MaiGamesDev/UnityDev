using UnityEngine;

public class Coin : MonoBehaviour
{
    public UIManager uiManager;
    public SoundManager soundManager;
    public AudioClip sndCoin;
    
    int value = 10;

    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            GetCoin();
    }
    void GetCoin()
    {
        //사운드 재생, 코인 획득
        soundManager.PlaySound(sndCoin);
        uiManager.StartCoroutine(uiManager.ShowNotice($"{value} 코인을 얻었다."));
        uiManager.SetGold(GameManager.Instance.gold + value);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            GetCoin();
    }
}
