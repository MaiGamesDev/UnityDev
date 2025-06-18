using UnityEngine;

public class Coin : MonoBehaviour
{
    public UIManager uiManager;
    int value = 10;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetCoin();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void GetCoin()
    {
        uiManager.StartCoroutine(uiManager.ShowNotice($"{value} 코인을 얻었다."));
        uiManager.SetGold(uiManager.gold + value);
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            GetCoin();
    }
}
