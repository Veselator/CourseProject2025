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
        _playerInput.playerInputAction.Player.Disable(); // Обязательно отключаем
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    public static void ExitToMenu()
    {
        _playerInput.playerInputAction.Player.Disable();
        Application.Quit();
    }

    public static void LoadNextScene()
    {
        // Временно!
        _playerInput.playerInputAction.Player.Disable();
        Application.Quit();
    }
}
