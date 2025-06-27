using UnityEngine;

//Game Manager ΩÃ±€≈Ê
public class GameManager : MonoBehaviour
{
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

    public float hp = 100f;
    public float maxHp = 100f;
    public float gold = 0;
    public float damage = 1;

    public int unlockedMapCount = 2;


}
