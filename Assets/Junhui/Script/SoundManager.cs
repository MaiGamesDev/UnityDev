using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource;


    public AudioClip introBgmClip;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playLoopSound(introBgmClip);
    }

    void playSound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    void playLoopSound(AudioClip clip)
    {
        audioSource.loop = true;
        audioSource.clip = clip;
        audioSource.Play();
    }
}
