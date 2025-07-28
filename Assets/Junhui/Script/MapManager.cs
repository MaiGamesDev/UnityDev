using UnityEngine;

public class MapManager : MonoBehaviour
{
    public AudioClip bgm;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SoundManager.Instance.PlayLoopSound(bgm);
    }

}
