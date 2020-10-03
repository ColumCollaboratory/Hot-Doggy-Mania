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

    public void LoadGameScene()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void ShowTitle()
    {
        titleMenuPanel.SetActive(true);
        creditsMenuPanel.SetActive(false);
    }

    public void ShowCredits()
    {
        creditsMenuPanel.SetActive(true);
        titleMenuPanel.SetActive(false);
    }

    public void ExitGame()
    {
        Debug.Log("Quit.");
        Application.Quit();
    }
}
