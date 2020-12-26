using UnityEngine;

/// <summary>
/// Base class for UI controls that bind control schemes.
/// </summary>
public abstract class BindingControl : MonoBehaviour
{
    #region Base Inspector Fields
    [Header("Binding Control")]
    [Tooltip("The input binding context that this control is linked to.")]
    [SerializeField] protected InputBindingContext context = null;
    #endregion
}
