using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemDropSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] coins;

    private void Awake()
    {
        coins = Resources.LoadAll<GameObject>("Coins");

        if (coins.Length == 0)
        {
            Debug.LogError("코인 프리팹이 Resources/Coins 폴더에 없습니다!");
        }
    }

    public void DropItem(Vector3 dropPos)
    {
        var randomIndex = Random.Range(0, coins.Length);

        GameObject item = Instantiate(coins[randomIndex], dropPos, Quaternion.identity);
        Rigidbody2D itemRb = item.GetComponent<Rigidbody2D>();

        itemRb.AddForceX(Random.Range(-2f, 2f), ForceMode2D.Impulse);
        itemRb.AddForceY(3f, ForceMode2D.Impulse);

        float ranPower = Random.Range(-1f, 1f);
        itemRb.AddTorque(ranPower, ForceMode2D.Impulse);
    }
}
