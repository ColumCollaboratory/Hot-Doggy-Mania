using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    #region JavaScript Interface
    [DllImport("__Internal")]
    private static extern void Quit();
    #endregion


    private void Start()
    {
        AudioSingleton.PlayBGM(BackgroundMusic.MainMenu);
    }

    public void ReturnMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
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
