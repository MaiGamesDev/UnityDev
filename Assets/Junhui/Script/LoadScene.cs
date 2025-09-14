using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private int mapCount = 0;
    [SerializeField] private bool noUI = false;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            ChangeScene();
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneName);
        if (mapCount != 0)
        {
            if (GameManager.Instance.unlockedMapCount < mapCount)
            {
                GameManager.Instance.unlockedMapCount = mapCount;
            }
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (UIManager.Instance != null)
        {
            if (noUI)
                UIManager.Instance.ShowUI();
            else
                UIManager.Instance.HideUI();

            UIManager.Instance.ResetHp();
            UIManager.Instance.HpEnemyParent.gameObject.SetActive(false);
        }

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
