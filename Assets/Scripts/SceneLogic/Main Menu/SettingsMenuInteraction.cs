using UnityEngine;
using UnityEngine.UI;

public sealed class SettingsMenuInteraction : MonoBehaviour
{
    [SerializeField] private ToggleButtonRenderer muteBGMRenderer = null;
    [SerializeField] private ToggleButtonRenderer muteSFXRenderer = null;
    [SerializeField] private ToggleSliderRenderer adjustBGMSlider = null;
    [SerializeField] private ToggleSliderRenderer adjustSFXSlider = null;

    [SerializeField] private Button muteBGMButton = null;
    [SerializeField] private Button muteSFXButton = null;
    [SerializeField] private Slider BGMSlider = null;
    [SerializeField] private Slider SFXSlider = null;

    [SerializeField] private SoundEffect testEffect = default;

    private float lastSFXTime;

    private void Awake()
    {
        // Bind to the UI controls.
        muteBGMButton.onClick.RemoveAllListeners();
        muteBGMButton.onClick.AddListener(ToggleBGM);
        BGMSlider.onValueChanged.RemoveAllListeners();
        BGMSlider.onValueChanged.AddListener(AdjustBGM);
        muteSFXButton.onClick.RemoveAllListeners();
        muteSFXButton.onClick.AddListener(ToggleSFX);
        SFXSlider.onValueChanged.RemoveAllListeners();
        SFXSlider.onValueChanged.AddListener(AdjustSFX);

        // Initialize the visual state
        // of the renderers. Singleton state is initially
        // inverted to undo the toggle function.
        AudioSingleton.BGMMuted = !AudioSingleton.BGMMuted;
        ToggleBGM();
    }

    private void AdjustBGM(float value)
    {
        AudioSingleton.BGMVolume = value;
    }
    private void ToggleBGM()
    {
        AudioSingleton.BGMMuted = !AudioSingleton.BGMMuted;
        muteBGMRenderer.IsToggled = !AudioSingleton.BGMMuted;
        adjustBGMSlider.IsToggled = !AudioSingleton.BGMMuted;
    }
    private void AdjustSFX(float value)
    {
        AudioSingleton.SFXVolume = value;
        if (Time.time - lastSFXTime > 1f)
        {
            lastSFXTime = Time.time;
            AudioSingleton.PlaySFX(testEffect);
        }
    }
    private void ToggleSFX()
    {
        AudioSingleton.SFXMuted = !AudioSingleton.SFXMuted;
        muteSFXRenderer.IsToggled = !AudioSingleton.SFXMuted;
        adjustSFXSlider.IsToggled = !AudioSingleton.SFXMuted;
    }
}
