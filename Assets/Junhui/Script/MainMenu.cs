using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioClip BGMMainMenu;

    [SerializeField] private string sceneName;
    [SerializeField] private AudioClip BGM;
    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneName);
        if (BGM != null)
        {
            SoundManager.Instance.PlayLoopSound(BGM);
        }
    }   

    public void quit()
    {
        Application.Quit();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SoundManager.Instance.PlayLoopSound(BGMMainMenu);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
