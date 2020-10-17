using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine;

public sealed class AudioSingleton : MonoBehaviour
{
    public static AudioSingleton instance;

    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private string[] clipNames;
    [SerializeField] private GameObject muteButton;

    private Dictionary<string, AudioClip> clips;
    private float sfxVolume;
    private float bgmVolume;
    private bool isMuted=false;
    private bool isRamping = false;
    private Color muteButtonColor;
    private Color muteGray = new Color(220, 220, 220);
    

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
            sfxVolume = sfxSource.volume;
            bgmVolume = bgmSource.volume;
            muteButtonColor = muteButton.GetComponent<Image>().color;
            RampVolume();
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
    public void RampVolume()
    {
        bgmSource.volume = 0;
        isRamping = true;
    }
    public void ToggleMute()
    {
        if(isMuted==true)
        {
            sfxSource.volume = sfxVolume;
            bgmSource.volume = bgmVolume;
            muteButton.GetComponent<Image>().color=muteButtonColor;
            isMuted = false;
        }
        else
        {
            sfxSource.volume = 0;
            bgmSource.volume = 0;
            isMuted = true;
            muteButton.GetComponent<Image>().color=muteGray;
        }
    }

    public void Mute(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            ToggleMute();
        }
    }

    private void Update()
    {
        if(isRamping==true&&isMuted==false)
        {
            if (bgmSource.volume < bgmVolume)
            {
                bgmSource.volume = bgmSource.volume + 0.1f * Time.deltaTime;
            }
            else
            {
                isRamping = false;
            }
        }
    }
}
