using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private AudioClip BGM;
    [SerializeField] private int mapCount = 0;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            ChangeScene();
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneName);
        if (BGM != null)
        {
            SoundManager.Instance.PlayLoopSound(BGM);
        }
        if (mapCount != 0)
        {
            if (GameManager.Instance.unlockedMapCount < mapCount)
            {
                GameManager.Instance.unlockedMapCount = mapCount;
            }
        }

    }
}
