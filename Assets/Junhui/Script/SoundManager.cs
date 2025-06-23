using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource;


    public AudioClip introBgmClip;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

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
