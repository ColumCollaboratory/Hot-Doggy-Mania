using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class InputBindingSelect : BindingControl
{
    [SerializeField] private Sprite selectedButtonBackground = null;
    [SerializeField] private Sprite unselectedButtonBackground = null;
    [SerializeField] private SelectionBindings[] selectionFields = null;
    [Serializable]
    private sealed class SelectionBindings
    {
        public Button button;
        public BindingPair[] bindings;
    }
    [Serializable]
    private struct BindingPair
    {
        public string action;
        public string path;
    }


    // Start is called before the first frame update
    private void Start()
    {
        foreach (SelectionBindings pair in selectionFields)
        {
            pair.button.onClick.AddListener(() =>
            {
                foreach (SelectionBindings subPair in selectionFields)
                    subPair.button.GetComponent<Image>().sprite = unselectedButtonBackground;
                pair.button.GetComponent<Image>().sprite = selectedButtonBackground;
                foreach (BindingPair binding in pair.bindings)
                    context.TrySetBinding(binding.action, binding.path);
            });
        }
    }
}
