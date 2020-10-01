using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    void Awake() 
    {
        if (instance != null)
        {
            Destroy(gameObject);
        } 

        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } 
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadCredits()
    {
        // This number will have to change to whatever our credits scene is in the build index
        SceneManager.LoadScene(2);
    }

    public void QuitApplication()
    {
        // TODO Add a "Are you sure?" question before quitting
        Application.Quit();
    }

}
