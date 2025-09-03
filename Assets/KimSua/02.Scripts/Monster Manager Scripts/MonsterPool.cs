using UnityEngine;
using UnityEngine.Pool;

public class MonsterPool : MonoBehaviour
{
    private ObjectPoolQueue pool; // 돌아갈 풀장    

    void Awake()
    {
        pool = FindFirstObjectByType<ObjectPoolQueue>();
    }

    public void ReturnPool(GameObject obj) // 풀에 돌려보냄
    {
        pool.EnqueueObject(obj);
        obj.SetActive(false);
    }
}
