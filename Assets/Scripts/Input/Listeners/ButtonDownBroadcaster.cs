using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Wraps the new input system providing a broadcaster when a button is pressed down.
/// </summary>
public sealed class ButtonDownBroadcaster : MonoBehaviour
{
    #region Exposed Listener Property
    /// <summary>
    /// Called on the first frame that this button is down.
    /// </summary>
    public Action Listener
    {
        private get; set;
    }
    #endregion
    #region New Input Implementation
    /// <summary>
    /// Reciever for the new input system action call.
    /// </summary>
    /// <param name="context">The context of the input change.</param>
    public void OnButtonAction(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton() && !context.performed)
            Listener?.Invoke();
    }
    #endregion
}
