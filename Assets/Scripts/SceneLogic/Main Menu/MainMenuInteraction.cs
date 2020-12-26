using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class MainMenuInteraction : MonoBehaviour
{
    #region JavaScript Interface
    [DllImport("__Internal")]
    private static extern void Quit();
    #endregion
    #region Inspector Fields
    [Tooltip("The name of the first stage to load on play.")]
    [SerializeField] private string firstStageScene = string.Empty;
    [Tooltip("The root object of the title content.")]
    [SerializeField] private GameObject titlePanel = null;
    [Tooltip("The root object of the settings content.")]
    [SerializeField] private GameObject settingsPanel = null;
    [Tooltip("The root object of the credits content.")]
    [SerializeField] private GameObject creditsPanel = null;
    [Tooltip("The root object of the instructions content.")]
    [SerializeField] private GameObject instructionsPanel = null;
    [Tooltip("The animator that drives the overlay elements.")]
    [SerializeField] private Animator titleAnimator = null;
    [Tooltip("The script that controlls the credit scrolling behaviour.")]
    [SerializeField] private CreditsScroll creditsScroll = null;
    #endregion

    public void LoadGameScene()
    {
        SceneManager.LoadScene(firstStageScene);
        AudioSingleton.PlayBGM(BackgroundMusic.Gameplay);
    }
    public void ShowTitle()
    {
        titlePanel.SetActive(true);
        AudioSingleton.PlaySFX(SoundEffect.CloseCredits);
        instructionsPanel.SetActive(false);
        creditsPanel.SetActive(false);
        settingsPanel.SetActive(false);
        titleAnimator.SetTrigger("MaximizeTitle");
        creditsScroll.StopScrolling();
    }
    public void ShowCredits()
    {
        AudioSingleton.PlaySFX(SoundEffect.OpenCredits);
        creditsPanel.SetActive(true);
        titlePanel.SetActive(false);
        titleAnimator.SetTrigger("MinimizeTitle");
        creditsScroll.StartScrolling();
    }
    public void ShowInstructions()
    {
        AudioSingleton.PlaySFX(SoundEffect.CloseCredits);
        titlePanel.SetActive(false);
        instructionsPanel.SetActive(true);
        titleAnimator.SetTrigger("MinimizeTitle");
    }
    public void ShowSettings()
    {
        AudioSingleton.PlaySFX(SoundEffect.CloseCredits);
        titlePanel.SetActive(false);
        settingsPanel.SetActive(true);
        titleAnimator.SetTrigger("MinimizeTitle");
    }

    public void ExitGame()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.OSXEditor:
            case RuntimePlatform.LinuxEditor:
            case RuntimePlatform.WindowsEditor:
                Debug.Log("Exited");
                break;
            case RuntimePlatform.WebGLPlayer:
                Quit();
                break;
            default:
                Application.Quit();
                break;
        }
    }
}
