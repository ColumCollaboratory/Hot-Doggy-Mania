using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Encapsulates UI components to render a toggle state slider.
/// </summary>
public sealed class ToggleSliderRenderer : MonoBehaviour
{
    #region Inspector Fields
    [Tooltip("The associated slider.")]
    [SerializeField] private Slider slider = null;
    [Tooltip("The handle of the slider.")]
    [SerializeField] private Image sliderHandle = null;
    [Tooltip("The sprite used for the handle when the toggle is on.")]
    [SerializeField] private Sprite onHandleSprite = null;
    [Tooltip("The sprite used for the handle when the toggle is off.")]
    [SerializeField] private Sprite offHandleSprite = null;
    [Tooltip("The background of the slider.")]
    [SerializeField] private Image sliderBackground = null;
    [Tooltip("The sprite used for the background when the toggle is on.")]
    [SerializeField] private Sprite onBackgroundSprite = null;
    [Tooltip("The sprite used for the background when the toggle is off.")]
    [SerializeField] private Sprite offBackgroundSprite = null;
    [Tooltip("The filled region of the slider.")]
    [SerializeField] private Image sliderFill = null;
    [Tooltip("The sprite used for the fill when the toggle is on.")]
    [SerializeField] private Sprite onFillSprite = null;
    [Tooltip("The sprite used for the fill when the toggle is off.")]
    [SerializeField] private Sprite offFillSprite = null;
    [Tooltip("Sets the initialized rendered state of this renderer.")]
    [SerializeField] private bool isToggled = false;
    #endregion
    #region Properties
    /// <summary>
    /// Whether this toggle slider appears toggled to the user.
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
                    sliderHandle.sprite = onHandleSprite;
                    sliderBackground.sprite = onBackgroundSprite;
                    sliderFill.sprite = onFillSprite;
                    slider.enabled = true;
                }
                else
                {
                    sliderHandle.sprite = offHandleSprite;
                    sliderBackground.sprite = offBackgroundSprite;
                    sliderFill.sprite = offFillSprite;
                    slider.enabled = false;
                }
            }
        }
    }
    #endregion
    #region Initialization
    private void Awake()
    {
        // Set the initial visual state of the renderer.
        IsToggled = isToggled;
    }
    #endregion
}
