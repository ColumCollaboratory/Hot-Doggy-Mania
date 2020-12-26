using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Encapsulates UI components to render a toggle state button.
/// </summary>
public sealed class ToggleButtonRenderer : MonoBehaviour
{
    #region Private Fields
    private Image buttonBorderImage;
    #endregion
    #region Inspector Fields
    [Tooltip("The associated button")]
    [SerializeField] private Button button = null;
    [Tooltip("The sprite used for the button border when the toggle is on.")]
    [SerializeField] private Sprite onBorderSprite = null;
    [Tooltip("The sprite used for the button border when the toggle is off.")]
    [SerializeField] private Sprite offBorderSprite = null;
    [Tooltip("The icon inside the button that changes on toggle.")]
    [SerializeField] private Image buttonIconImage = null;
    [Tooltip("The sprite used for the icon when the toggle is on.")]
    [SerializeField] private Sprite onIconSprite = null;
    [Tooltip("The sprite used for the icon when the toggle is off.")]
    [SerializeField] private Sprite offIconSprite = null;
    [Tooltip("Sets the initialized rendered state of this renderer.")]
    [SerializeField] private bool isToggled = false;
    #endregion
    #region Properties
    /// <summary>
    /// Whether this toggle button appears toggled to the user.
    /// </summary>
    public bool IsToggled
    {
        get { return isToggled; }
        set
        {
            // Update the UI elements rendering state
            // if a new toggled state has been pushed.
            if (isToggled != value)
            {
                isToggled = value;
                if (isToggled)
                {
                    buttonBorderImage.sprite = onBorderSprite;
                    buttonIconImage.sprite = onIconSprite;
                }
                else
                {
                    buttonBorderImage.sprite = offBorderSprite;
                    buttonIconImage.sprite = offIconSprite;
                }
            }
        }
    }
    #endregion
    #region Initialization
    private void Awake()
    {
        buttonBorderImage = button.GetComponent<Image>();
        // Set the initial visual state of the renderer.
        IsToggled = isToggled;
    }
    #endregion
}
