using UnityEngine;

public class Coin : MonoBehaviour
{
    public UIManager uiManager;
    public SoundManager soundManager;
    public AudioClip sndCoin;
    
    int value = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            GetCoin();
    }
    void GetCoin()
    {
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
