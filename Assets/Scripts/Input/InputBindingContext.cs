using UnityEngine;
using UnityEngine.InputSystem;

public sealed class InputBindingContext : MonoBehaviour
{
    [SerializeField] private InputActionAsset actionMap = null;

    public void TrySetBinding(string bindingName, string newPath)
    {
        InputAction action = actionMap.FindAction(bindingName);
        if (action == null)
            Debug.LogWarning($"Tried to set non existant action: {bindingName}");
        else
            action.ApplyBindingOverride(new InputBinding(newPath));
    }
}
