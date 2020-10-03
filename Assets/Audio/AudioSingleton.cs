using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class AudioSingleton : MonoBehaviour
{
    public static AudioSingleton instance;

    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private string[] clipNames;

    private Dictionary<string, AudioClip> clips;

    private void Start()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
            if (audioClips.Length == clipNames.Length)
            {
                clips = new Dictionary<string, AudioClip>();
                for (int i = 0; i < clipNames.Length; i++)
                    clips.Add(clipNames[i], audioClips[i]);
            }
            bgmSource.loop = true;
        }
    }

    public void PlaySFX(string clipName)
    {
        sfxSource.PlayOneShot(clips[clipName]);
    }
    public void PlayBGM(string clipName)
    {
        StopBGM();
        bgmSource.clip = clips[clipName];
        bgmSource.Play();
    }
    public void StopBGM()
    {
        bgmSource.Stop();
    }
}
