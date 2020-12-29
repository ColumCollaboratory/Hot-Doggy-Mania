using UnityEngine;

public sealed class ResetGame : MonoBehaviour
{
    private void Start()
    {
        Score.score = 0;
        FindObjectOfType<MainMenuInteraction>().MainMenuSceneLoaded();
    }
}
