using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private AudioClip BGM; 
    public void OnButtonPressed()
    {
        SceneManager.LoadScene(sceneName);
        if (BGM != null)
        {
            SoundManager.Instance.PlayLoopSound(BGM);
        }
    }
}
