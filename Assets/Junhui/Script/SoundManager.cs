using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;


    public static SoundManager Instance
    {
        get
        {
            if (instance == null) instance = new SoundManager();
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

    public AudioSource audioSource;

    public void PlaySound(AudioClip clip)
    {
        // 사운드 재생
        audioSource.PlayOneShot(clip);
    }

    public void PlayLoopSound(AudioClip clip)
    {
        // BGM 재생
        audioSource.loop = true;
        audioSource.clip = clip;
        audioSource.Play();
    }
}
