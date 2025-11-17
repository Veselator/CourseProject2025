using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private PauseControllManager pauseControllManager;

    private void OnEnable()
    {
        pauseControllManager.OnGamePauseChanged += HandleGamePauseChanged;
        pauseMenuUI.SetActive(false);
    }

    private void OnDisable()
    {
        pauseControllManager.OnGamePauseChanged -= HandleGamePauseChanged;
    }

    private void HandleGamePauseChanged(bool isPaused)
    {
        pauseMenuUI.SetActive(isPaused);
    }
}
