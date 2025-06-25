using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private AudioClip BGM; 


    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("ss");
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
    }
}
