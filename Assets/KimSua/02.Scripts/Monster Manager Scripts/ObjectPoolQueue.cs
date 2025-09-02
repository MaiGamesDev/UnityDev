using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolQueue : MonoBehaviour
{
    private Queue<GameObject> objQueue = new Queue<GameObject>();

    [SerializeField] private List<GameObject> monsterPrefabs;
    [SerializeField] private Transform parent;

    [SerializeField] private int initialCount = 10;

    void Start()
    {
        CreateObject();
    }

    void CreateObject() // 오브젝트 생성 -> Pool 채움
    {
        for (int i = 0; i < initialCount; i++)
        {
            int rand = Random.Range(0, monsterPrefabs.Count);
            GameObject prefab = monsterPrefabs[rand];

            GameObject obj = Instantiate(prefab, parent); // 오브젝트 생성, parent 자식으로 변경
            obj.SetActive(false);

            EnqueueObject(obj);
        }        
    }

    public void EnqueueObject(GameObject newObj) // 보관
    {
        newObj.SetActive(false);
        objQueue.Enqueue(newObj);
    }

    public GameObject DequeueObject(Vector3 pos, Quaternion rot) // 꺼냄
    {
        if (objQueue.Count <= 5)
        {
            CreateObject();
        }

        GameObject objToUse = objQueue.Dequeue();

        if (objToUse.CompareTag("Fly"))        
            pos.y = 0f; // Fly는 항상 y=0        
        else        
            pos.y = -3f; // 지상 몬스터
        

        objToUse.transform.SetPositionAndRotation(pos, rot);
        objToUse.SetActive(true);

        return objToUse;
    }
}
