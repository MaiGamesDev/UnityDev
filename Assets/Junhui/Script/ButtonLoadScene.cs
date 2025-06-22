using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonLoadScene : MonoBehaviour
{
    public string sceneName;
    public void OnButtonPressed()
    {
        SceneManager.LoadScene(sceneName);
    }
}
