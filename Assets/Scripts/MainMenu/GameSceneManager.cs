using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class GameSceneManager : MonoBehaviour
{
    // Менеджер загрузок

    private static GameSceneManager _instance;
    public static GameSceneManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("GameSceneManager не найден в сцене!");
            }
            return _instance;
        }
    }

    [Header("Настройки сцен")]
    [SerializeField] private string _mainMenuSceneName = "MainMenu";
    [SerializeField] private string[] _levelScenes;

    [Header("Настройки загрузки")]
    [SerializeField] private bool _useAsyncLoading = true;
    [SerializeField] private float _minimumLoadingTime = 0.5f;
    public float LoadingTime => _minimumLoadingTime;

    private int _currentLevelIndex = -1;

    private GameAudioManager _audioManager;

    public event Action<float> OnLoadingProgress;
    public event Action<int, bool> OnLoadingStarted;
    public event Action<int> OnLoadingCompleted;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        Debug.Log("GameSceneManager инициализирован");
    }

    private void Start()
    {
        _audioManager = GameAudioManager.Instance;
    }

    public static void ReloadScene()
    {
        Instance?.ReloadCurrentScene();
    }

    public static void ExitToMenu()
    {
        Debug.Log("GameSceneManager: Выход в меню...");
        Instance?.LoadMainMenu();
    }

    public static void LoadNextScene()
    {
        Debug.Log("GameSceneManager: Загружаю следующую сцену...");
        Instance?.LoadNextLevel();
    }

    public void LoadLevel(int levelIndex)
    {
        if (!IsValidLevelIndex(levelIndex))
        {
            Debug.LogError($"Неверный индекс уровня: {levelIndex}. Доступно уровней: {_levelScenes.Length}");
            return;
        }

        _currentLevelIndex = levelIndex;
        LoadSceneByName(_levelScenes[levelIndex]);
    }

    public void LoadLevelByName(string sceneName)
    {
        for (int i = 0; i < _levelScenes.Length; i++)
        {
            if (_levelScenes[i] == sceneName)
            {
                _currentLevelIndex = i;
                break;
            }
        }

        LoadSceneByName(sceneName);
    }

    private void LoadNextLevel()
    {
        if (_currentLevelIndex < 0)
        {
            Debug.LogWarning("Невозможно загрузить следующий уровень - мы не в уровне");
            return;
        }

        int nextLevelIndex = _currentLevelIndex + 1;

        if (nextLevelIndex >= _levelScenes.Length)
        {
            Debug.Log("Это был последний уровень! Возвращаемся в меню");
            LoadMainMenu();
            return;
        }

        LoadLevel(nextLevelIndex);
    }

    private void ReloadCurrentScene()
    {
        Debug.Log($"Перезагрузка сцены: {CurrentSceneName}");
        LoadSceneByName(CurrentSceneName);
    }

    private void LoadMainMenu()
    {
        _currentLevelIndex = -1;
        LoadSceneByName(_mainMenuSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Выход из игры...");
        Application.Quit();
    }

    private bool IsValidLevelIndex(int index)
    {
        return index >= 0 && index < _levelScenes.Length;
    }

    private void LoadSceneByName(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Имя сцены пустое!");
            return;
        }

        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            if (_useAsyncLoading)
            {
                StartCoroutine(LoadSceneAsync(sceneName));
            }
            else
            {
                OnLoadingStarted?.Invoke(_currentLevelIndex, false);
                SceneManager.LoadScene(sceneName);
            }
        }
        else
        {
            Debug.LogError($"Сцена '{sceneName}' не найдена в Build Settings!");
        }
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        //Debug.Log("LoadSceneAsync");
        OnLoadingStarted?.Invoke(_currentLevelIndex, true);

        float startTime = Time.time;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            OnLoadingProgress?.Invoke(progress);

            if (asyncLoad.progress >= 0.9f)
            {
                float elapsedTime = Time.time - startTime;
                if (elapsedTime >= _minimumLoadingTime)
                {
                    OnLoadingProgress?.Invoke(1f);
                    asyncLoad.allowSceneActivation = true;
                }
            }

            yield return null;
        }

        OnLoadingCompleted?.Invoke(_currentLevelIndex);
        Debug.Log($"Сцена '{sceneName}' успешно загружена");
    }

    public int CurrentSceneIndex => _currentLevelIndex;
    public bool IsInLevel => _currentLevelIndex >= 0;
    public int TotalScenes => _levelScenes.Length;
    public string CurrentSceneName => SceneManager.GetActiveScene().name;
}