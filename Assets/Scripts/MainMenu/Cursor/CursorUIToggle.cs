using System;
using UnityEngine;
using UnityEngine.UI;

public class CursorUIToggle : MonoBehaviour
{
    [SerializeField] private Toggle _linkedToggle;

    public event Action OnToggled;

    private void Start()
    {
        _linkedToggle.isOn = CursorManager.Instance.IsDynamicCursor;
    }

    public void Toggle()
    {
        OnToggled?.Invoke();
        CursorManager.Instance.ToggleDynamicCursor();
    }
}
