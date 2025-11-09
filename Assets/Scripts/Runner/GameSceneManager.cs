using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    private static PlayerInput _playerInput;

    private void Start()
    {
        _playerInput = PlayerInput.Instance;
    }

    public static void ReloadScene()
    {
        if (_playerInput == null) Debug.Log("PLAYER INPUT IS NULL");
        else _playerInput.playerInputAction.Player.Disable();
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    public static void ExitToMenu()
    {
        Debug.Log("GameSceneManager: Выход в меню...");
        Application.Quit();
    }

    public static void LoadNextScene()
    {
        // Временно!
        Debug.Log("GameSceneManager: Загружаю следующую сцену...");
        Application.Quit();
    }
}
