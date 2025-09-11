using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemDropSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] coins;

    private void Awake()
    {
        if (coins.Length == 0)
        {
            Debug.LogError("코인 프리팹이 없습니다!");
        }
    }

    public void DropItem(Vector3 dropPos, GameObject owner)
    {
        Debug.Log($"DropItem 호출됨 - 위치: {dropPos}, 오너: {owner.name}");
        string layerName = LayerMask.LayerToName(owner.layer);
        Debug.Log($"오너 레이어: {layerName} (레이어 번호: {owner.layer})");

        int dropCount = 1;

        if (layerName == "Boss")
        {
            dropCount = Random.Range(3, 11); // 3 ~ 10개
        }
        else if (layerName == "Monster")
        {
            dropCount = Random.Range(1, 4);
        }

        Debug.Log($"드롭할 아이템 개수: {dropCount}");

        // coins 배열 상태 확인
        if (coins == null || coins.Length == 0)
        {
            Debug.LogError("coins 배열이 null이거나 비어있습니다!");
            return;
        }

        for (int i = 0; i < dropCount; i++)
        {
            var randomIndex = Random.Range(0, coins.Length);

            GameObject item = Instantiate(coins[randomIndex], dropPos, Quaternion.identity);
            Debug.Log($"아이템 생성됨: {item.name}");
            Rigidbody2D itemRb = item.GetComponent<Rigidbody2D>();

            float angle = Random.Range(0f, 360f);
            float power = Random.Range(5f, 10f);

            // Cos: 각도의 x축 방향 비율, Sin: 각도의 y축 방향 비율
            // Deg2Rad: rad 값 -> Degree 변환
            Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            itemRb.AddForce(dir * power, ForceMode2D.Impulse);
            itemRb.AddTorque(power, ForceMode2D.Impulse);
        }           
    }
}
