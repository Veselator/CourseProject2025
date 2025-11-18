using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    // Ну здравствуй, человек по ту сторону экрана
    // Удивлён, зачем эта сцена и этот скрипт нужен?
    // Это прикол с синглтоном и DontDestroyOnLoad объектами
    // Короче, долго объяснять, можешь погуглить если что
    // Solomka 17.11.25

    private void Start()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
