using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    private void Start()
    {
        AudioSingleton.instance?.PlayBGM("Menu_BGM");
    }

    public void ReturnMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Debug.Log("Quit.");
        Application.Quit();
    }
}
