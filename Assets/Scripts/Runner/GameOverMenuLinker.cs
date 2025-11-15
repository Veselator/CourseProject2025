using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverMenuLinker : MonoBehaviour
{
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _mainMenuButton;

    private void Start()
    {
        _restartButton.onClick.AddListener(RestartLevel);
        _mainMenuButton.onClick.AddListener(ReturnToMainMenu);
    }

    private void OnDestroy()
    {
        _restartButton.onClick.RemoveListener(RestartLevel);
        _mainMenuButton.onClick.RemoveListener(ReturnToMainMenu);
    }

    private void RestartLevel()
    {
        GameSceneManager.ReloadScene();
    }

    private void ReturnToMainMenu()
    {
        GameSceneManager.ExitToMenu();
    }
}
