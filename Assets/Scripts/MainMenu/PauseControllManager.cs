using System;
using UnityEngine;

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

    public void TogglePause()
    {
        IsPaused = !IsPaused;
        Time.timeScale = IsPaused ? 0f : 1f;
    }
}
