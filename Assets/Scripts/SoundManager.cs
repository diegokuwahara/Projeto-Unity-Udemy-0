using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource fxSource;
    public AudioSource musicSource;

    public static SoundManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(base.gameObject);
        }

        DontDestroyOnLoad(base.gameObject);
    }
    
    public void PlaySound(AudioClip audioClip)
    {
        fxSource.clip = audioClip;
        fxSource.Play();
    }
}
