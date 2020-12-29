using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public sealed class MainMenuInteraction : MonoBehaviour
{
    #region JavaScript Interface
    [DllImport("__Internal")]
    private static extern void Quit();
    [DllImport("__Internal")]
    private static extern void SetFullscreen(bool isFullscreen);
    #endregion
    #region Inspector Fields
    [Tooltip("The canvas containing the menu widgets.")]
    [SerializeField] private Canvas menuCanvas = null;
    [Tooltip("The event system that is driven by the controller.")]
    [SerializeField] private EventSystem controlEventSystem = null;
    [Tooltip("The animator that drives the overlay elements.")]
    [SerializeField] private Animator titleAnimator = null;
    [Tooltip("The animator that drives the pause elements.")]
    [SerializeField] private Animator pauseAnimator = null;
    [Tooltip("The renderer for the fullscreen button.")]
    [SerializeField] private ToggleButtonRenderer fullscreenButton = null;
    [Tooltip("Binding to the new input system using the pause button.")]
    [SerializeField] private ButtonDownBroadcaster pauseBroadcaster = null;
    [Header("Main Title Panel")]
    [Tooltip("The root object of the title content.")]
    [SerializeField] private GameObject titlePanel = null;
    [Tooltip("The item that initially has control focus on this panel.")]
    [SerializeField] private GameObject titleDefaultFocus = null;
    [Tooltip("The name of the first stage to load on play.")]
    [SerializeField] private string firstStageScene = string.Empty;
    [Header("Settings Panel")]
    [Tooltip("The root object of the settings content.")]
    [SerializeField] private GameObject settingsPanel = null;
    [Tooltip("The item that initially has control focus on this panel.")]
    [SerializeField] private GameObject settingsDefaultFocus = null;
    [Header("Credits Panel")]
    [Tooltip("The root object of the credits content.")]
    [SerializeField] private GameObject creditsPanel = null;
    [Tooltip("The item that initially has control focus on this panel.")]
    [SerializeField] private GameObject creditsDefaultFocus = null;
    [Tooltip("The script that controlls the credit scrolling behaviour.")]
    [SerializeField] private CreditsScroll creditsScroll = null;
    [Header("Instructions Panel")]
    [Tooltip("The root object of the instructions content.")]
    [SerializeField] private GameObject instructionsPanel = null;
    [Tooltip("The item that initially has control focus on this panel.")]
    [SerializeField] private GameObject instructionsDefaultFocus = null;
    #endregion

    private bool isFullscreen;
    private bool inGame;
    private bool isPaused;

    private void Awake()
    {
        pauseBroadcaster.Listener = OnPausePressed;
    }

    public void MainMenuSceneLoaded()
    {
        inGame = false;
        isPaused = false;
        menuCanvas.GetComponent<GraphicRaycaster>().enabled = true;
        pauseAnimator.SetBool("isPaused", true);
        titleAnimator.SetBool("isPaused", true);
        Time.timeScale = 0f;
        controlEventSystem.SetSelectedGameObject(titleDefaultFocus);
    }


    public void ToggleFullscreen()
    {
        isFullscreen = !isFullscreen;
        fullscreenButton.IsToggled = isFullscreen;
        switch (Application.platform)
        {
            case RuntimePlatform.OSXEditor:
            case RuntimePlatform.LinuxEditor:
            case RuntimePlatform.WindowsEditor:
                Debug.Log("Fullscreen Toggled");
                break;
            case RuntimePlatform.WebGLPlayer:
                SetFullscreen(isFullscreen);
                break;
            default:
                Screen.fullScreen = true;
                break;
        }
    }

    public void OnPlayPressed()
    {
        if (inGame)
            OnPausePressed();
        else
        {
            inGame = true;
            isPaused = false;
            SceneManager.LoadScene(firstStageScene);
            AudioSingleton.PlayBGM(BackgroundMusic.Gameplay);
            pauseAnimator.SetBool("isPaused", false);
            titleAnimator.SetBool("isPaused", false);
            Time.timeScale = 1f;
            menuCanvas.GetComponent<GraphicRaycaster>().enabled = false;
            controlEventSystem.SetSelectedGameObject(null);
        }
    }

    public void OnPausePressed()
    {
        if (inGame)
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                pauseAnimator.SetBool("isPaused", true);
                titleAnimator.SetBool("isPaused", true);
                Time.timeScale = 0f;
                menuCanvas.GetComponent<GraphicRaycaster>().enabled = true;
                controlEventSystem.SetSelectedGameObject(titleDefaultFocus);
            }
            else
            {
                pauseAnimator.SetBool("isPaused", false);
                titleAnimator.SetBool("isPaused", false);
                Time.timeScale = 1f;
                menuCanvas.GetComponent<GraphicRaycaster>().enabled = false;
                controlEventSystem.SetSelectedGameObject(null);
            }
        }
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
        controlEventSystem.SetSelectedGameObject(titleDefaultFocus);
    }
    public void ShowCredits()
    {
        AudioSingleton.PlaySFX(SoundEffect.OpenCredits);
        creditsPanel.SetActive(true);
        titlePanel.SetActive(false);
        titleAnimator.SetTrigger("MinimizeTitle");
        creditsScroll.StartScrolling();
        controlEventSystem.SetSelectedGameObject(creditsDefaultFocus);
    }
    public void ShowInstructions()
    {
        AudioSingleton.PlaySFX(SoundEffect.CloseCredits);
        titlePanel.SetActive(false);
        instructionsPanel.SetActive(true);
        titleAnimator.SetTrigger("MinimizeTitle");
        controlEventSystem.SetSelectedGameObject(instructionsDefaultFocus);
    }
    public void ShowSettings()
    {
        AudioSingleton.PlaySFX(SoundEffect.CloseCredits);
        titlePanel.SetActive(false);
        settingsPanel.SetActive(true);
        titleAnimator.SetTrigger("MinimizeTitle");
        controlEventSystem.SetSelectedGameObject(settingsDefaultFocus);
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
