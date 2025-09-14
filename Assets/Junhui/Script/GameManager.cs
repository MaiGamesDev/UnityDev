using UnityEngine;

//Game Manager ½Ì±ÛÅæ
public class GameManager : MonoBehaviour
{
    [SerializeField] private ItemDropSpawner itemDropSpawner;
    public ItemDropSpawner ItemDropSpawner => itemDropSpawner;

    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (instance == null) instance = new GameManager();
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public float hp = 10f;
    public float maxHp = 10f;
    public float gold = 0;
    public float damage = 1;

    public int unlockedMapCount = 2;


}
