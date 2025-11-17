using UnityEngine;
using UnityEngine.UI;

public class PauseBUttonsLinker : MonoBehaviour
{
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _backButton;
    [SerializeField] private PauseControllManager _pauseControllManager;

    private void Start()
    {
        _restartButton.onClick.AddListener(Reload);
        _mainMenuButton.onClick.AddListener(ExitToMenu);
        _backButton.onClick.AddListener(_pauseControllManager.TogglePause);
    }

    private void OnDestroy()
    {
        _restartButton.onClick.RemoveListener(Reload);
        _mainMenuButton.onClick.RemoveListener(ExitToMenu);
        _backButton.onClick.RemoveListener(_pauseControllManager.TogglePause);
    }

    private void Reload()
    {
        _pauseControllManager.TogglePause();
        GameSceneManager.ReloadScene();
    }

    private void ExitToMenu()
    {
        _pauseControllManager.TogglePause();
        GameSceneManager.ExitToMenu();
    }
}
