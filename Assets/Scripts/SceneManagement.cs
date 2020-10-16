using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneManagement : MonoBehaviour
{
    [SerializeField]
    private string gameSceneName;
    [SerializeField]
    private GameObject creditsMenuPanel;
    [SerializeField]
    private GameObject titleMenuPanel;
    [SerializeField]
    private GameObject instructionsMenuPanel;

    private void Start()
    {

    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene(gameSceneName);
        AudioSingleton.instance?.PlayBGM("Gameplay_BGM");
    }

    public void ShowTitle()
    {
        titleMenuPanel.SetActive(true);
        AudioSingleton.instance?.PlaySFX("Credits_Close");
        instructionsMenuPanel.SetActive(false);
        creditsMenuPanel.SetActive(false);
    }

    public void ShowCredits()
    {
        AudioSingleton.instance?.PlaySFX("Credits_Open");
        creditsMenuPanel.SetActive(true);
        titleMenuPanel.SetActive(false);
    }

    public void ShowInstructions()
    {
        titleMenuPanel.SetActive(false);
        AudioSingleton.instance?.PlaySFX("Credits_Close");
        instructionsMenuPanel.SetActive(true);
    }

    public void ExitGame()
    {
        Debug.Log("Quit.");
        Application.Quit();
    }
}
