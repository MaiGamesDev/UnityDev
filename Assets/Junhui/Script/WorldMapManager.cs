using UnityEngine;
using UnityEngine.UI;

public class WorldMapManager : MonoBehaviour
{
    [SerializeField] private Button[] buttons;
    void Start()
    {
        for (int i = 0; i < GameManager.Instance.unlockedMapCount; i++)
        {
            buttons[i].gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
