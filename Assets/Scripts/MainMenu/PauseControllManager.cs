using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PauseControllManager : MonoBehaviour
{
    public event Action<bool> OnGamePauseChanged;
    private bool _isGamePaused = false;
    private bool IsPaused
    {
        get => _isGamePaused;
        set {
            if (_isGamePaused != value)
            {
                _isGamePaused = value;
                OnGamePauseChanged?.Invoke(_isGamePaused);
            }
        }
    }

    [SerializeField] private InputActionReference _keyToShowMenu;

    private void Start()
    {
        _keyToShowMenu.action.performed += TogglePause;
        _keyToShowMenu.action.Enable();
    }

    private void OnDestroy()
    {
        _keyToShowMenu.action.performed -= TogglePause;
        _keyToShowMenu.action.Disable();
    }

    private void TogglePause(InputAction.CallbackContext _)
    {
        TogglePause();
    }

    public void TogglePause()
    {
        IsPaused = !IsPaused;
        Time.timeScale = IsPaused ? 0f : 1f;
    }
}
