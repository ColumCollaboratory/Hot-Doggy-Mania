using System;
using System.Collections.Generic;
using UnityEngine;

#region Exposed Enums
/// <summary>
/// Identifies a sound effect track to play.
/// </summary>
public enum SoundEffect : byte
{
    LevelComplete,
    OrderComplete,
    OpenCredits,
    CloseCredits,
    DropIngredient,
    GarbageBin,
    LifeLost,
    SpraySalt,
    WrongIngredient,
    CorrectIngredient,
    EnemySquished
}
/// <summary>
/// Identifies a background music track to play.
/// </summary>
public enum BackgroundMusic : byte
{
    MainMenu,
    Gameplay
}
#endregion

/// <summary>
/// Handles non-spatial audio requests.
/// </summary>
public sealed class AudioSingleton : MonoBehaviour
{
    #region Fields
    // Stores a directory of all audio tracks that can be queried.
    private Dictionary<SoundEffect, AudioClip[]> sfxClips;
    private Dictionary<BackgroundMusic, AudioClip> bgmClips;
    // Stores state for the audio settings.
    private bool sfxMuted, bgmMuted;
    private float sfxVolume, bgmVolume;
    // Stores the instance referred to by static members.
    private static AudioSingleton instance;
    #endregion
    #region Inspector Fields
    [Tooltip("The audio source the plays sound effects.")]
    [SerializeField] private AudioSource sfxSource = null;
    [Tooltip("The audio source the plays background music.")]
    [SerializeField] private AudioSource bgmSource = null;
    [Tooltip("Defines the background tracks used during play.")]
    [SerializeField] private BGMClipPair[] backgroundTracks = null;
    [Tooltip("Defines the sound effect tracks used during play.")]
    [SerializeField] private SFXClipPair[] soundEffects = null;
    [Serializable]
    private sealed class BGMClipPair
    {
        [Tooltip("The name used to access this background music.")]
        public BackgroundMusic name = default;
        [Tooltip("The audioclip played for this background music.")]
        public AudioClip clip = null;
    }
    [Serializable]
    private sealed class SFXClipPair
    {
        [Tooltip("The name used to access this sound effect.")]
        public SoundEffect name = default;
        [Tooltip("The clips that can be played when this audio is requested.")]
        public AudioClip[] clips = null;
    }
    #endregion
    #region Singleton Initialization
    private void Awake()
    {
        // Enforce singleton.
        if (instance != null)
            Destroy(gameObject);
        else
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
            // Initialize collections to process inspector values.
            bgmClips = new Dictionary<BackgroundMusic, AudioClip>();
            sfxClips = new Dictionary<SoundEffect, AudioClip[]>();
            bool[] bgmClipsLinked
                = new bool[Enum.GetValues(typeof(BackgroundMusic)).Length];
            bool[] sfxClipsLinked
                = new bool[Enum.GetValues(typeof(SoundEffect)).Length];
            // Process the inspector values, watching for errors.
            foreach (BGMClipPair clipPair in backgroundTracks)
            {
                if (bgmClipsLinked[(int)clipPair.name] == true)
                    Debug.LogError($"Multiple clips are linked to BGM: {clipPair.name}");
                else
                {
                    bgmClipsLinked[(int)clipPair.name] = true;
                    bgmClips[clipPair.name] = clipPair.clip;
                }
            }
            foreach (SFXClipPair clipPair in soundEffects)
            {
                if (sfxClipsLinked[(int)clipPair.name] == true)
                    Debug.LogError($"Multiple clips are linked to SFX: {clipPair.name}");
                else
                {
                    sfxClipsLinked[(int)clipPair.name] = true;
                    sfxClips[clipPair.name] = clipPair.clips;
                }
            }
            // Log a warning if an enum value was not covered.
            for (int i = 0; i < bgmClipsLinked.Length; i++)
                if (!bgmClipsLinked[i])
                    Debug.LogWarning($"No BGM clip is linked to: {(BackgroundMusic)i}");
            for (int i = 0; i < sfxClipsLinked.Length; i++)
                if (!sfxClipsLinked[i])
                    Debug.LogWarning($"No SFX clip is linked to: {(SoundEffect)i}");
            // Set and pull in default values.
            bgmSource.loop = true;
            sfxVolume = sfxSource.volume;
            bgmVolume = bgmSource.volume;
        }
    }
    #endregion
    #region Audio Settings Accessors
    /// <summary>
    /// The volume of the background music (from 0 to 1).
    /// </summary>
    public static float BGMVolume
    {
        get { return instance.bgmVolume; }
        set
        {
            instance.bgmVolume = Mathf.Clamp01(value);
            instance.bgmSource.volume = instance.bgmVolume;
        }
    }
    /// <summary>
    /// The volume of the sound effects (from 0 to 1).
    /// </summary>
    public static float SFXVolume
    {
        get { return instance.sfxVolume; }
        set
        {
            instance.sfxVolume = Mathf.Clamp01(value);
            instance.sfxSource.volume = instance.sfxVolume;
        }
    }
    /// <summary>
    /// Whether the background music is muted.
    /// </summary>
    public static bool BGMMuted
    {
        get { return instance.bgmMuted; }
        set
        {
            instance.bgmMuted = value;
            if (value)
                instance.bgmSource.volume = 0f;
            else
                instance.bgmSource.volume = instance.bgmVolume;
        }
    }
    /// <summary>
    /// Whether the sound effects are muted.
    /// </summary>
    public static bool SFXMuted
    {
        get { return instance.sfxMuted; }
        set
        {
            instance.sfxMuted = value;
            if (value)
                instance.sfxSource.volume = 0f;
            else
                instance.sfxSource.volume = instance.sfxVolume;
        }
    }
    #endregion
    #region Audio Playing Methods
    /// <summary>
    /// Plays a sound effect once.
    /// </summary>
    /// <param name="clip">The sound effect clip to play.</param>
    public static void PlaySFX(SoundEffect clip)
    {
        instance.sfxSource.PlayOneShot(
            instance.sfxClips[clip].RandomElement());
    }
    /// <summary>
    /// Changes the background music track.
    /// </summary>
    /// <param name="track">The new background track to play.</param>
    public static void PlayBGM(BackgroundMusic track)
    {
        StopBGM();
        instance.bgmSource.clip = instance.bgmClips[track];
        instance.bgmSource.Play();
    }
    /// <summary>
    /// Stops the current background music track.
    /// </summary>
    public static void StopBGM()
    {
        instance.bgmSource.Stop();
    }
    #endregion
}
