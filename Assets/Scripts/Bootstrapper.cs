using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Advances the initialization scene to the first game scene.
/// </summary>
public sealed class Bootstrapper : MonoBehaviour
{
    #region Inspector Fields
    [Tooltip("Controls which objects in the hierarchy are preserved globally.")]
    [SerializeField] private GameObject dontDestroyParent = null;
    [Tooltip("The first scene to go to after initialization.")]
    [SerializeField] private string firstScene = string.Empty;
    #endregion
    #region Bootstrapping
    private void Start()
    {
        DontDestroyOnLoad(dontDestroyParent);
        SceneManager.LoadScene(firstScene);
    }
    #endregion
}
