using System.Linq;
using UnityEngine;

public class GameSaveManager : MonoBehaviour
{
    private static GameSaveManager _instance;
    public static GameSaveManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("GameSaveManager не найден в сцене!");
            }
            return _instance;
        }
    }

    private const string SAVE_KEY = "LevelProgress";
    private const int TOTAL_LEVELS = 7;
    public int TotalLevels => TOTAL_LEVELS;
    public int CompletedLevelCount => _levelCompleted.Count(c => c);
    public float CompletedPercentage => (float)CompletedLevelCount / (float)TotalLevels;

    private bool[] _levelCompleted;

    public bool this[int index]
    {
        get
        {
            if(index < 0 || index >= TotalLevels) return false;
            return _levelCompleted[index];
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        _levelCompleted = new bool[TOTAL_LEVELS];
        Load();

        Debug.Log("GameSaveManager инициализирован");
    }

    public void Save()
    {
        string saveData = BoolArrayToString(_levelCompleted);
        PlayerPrefs.SetString(SAVE_KEY, saveData);
        PlayerPrefs.Save();

        Debug.Log($"Прогресс сохранён: {saveData}");
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey(SAVE_KEY))
        {
            string saveData = PlayerPrefs.GetString(SAVE_KEY);
            _levelCompleted = StringToBoolArray(saveData);

            Debug.Log($"Прогресс загружен: {saveData}");
        }
        else
        {
            _levelCompleted = new bool[TOTAL_LEVELS];
            Debug.Log("Сохранений не найдено, создан новый прогресс");
        }
    }

    public void CleanUp()
    {
        _levelCompleted = new bool[TOTAL_LEVELS];
        PlayerPrefs.DeleteKey(SAVE_KEY);
        PlayerPrefs.Save();

        Debug.Log("Прогресс полностью сброшен");
    }

    public bool IsLevelCompleted(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= TOTAL_LEVELS)
        {
            Debug.LogError($"Неверный индекс уровня: {levelIndex}");
            return false;
        }

        return _levelCompleted[levelIndex];
    }

    public void SetLevelCompleted(int levelIndex, bool completed = true)
    {
        if (levelIndex < 0 || levelIndex >= TOTAL_LEVELS)
        {
            Debug.LogError($"Неверный индекс уровня: {levelIndex}");
            return;
        }

        _levelCompleted[levelIndex] = completed;
        Save();

        Debug.Log($"Уровень {levelIndex} отмечен как {(completed ? "пройденный" : "не пройденный")}");
    }

    private string BoolArrayToString(bool[] array)
    {
        string result = "";
        foreach (bool value in array)
        {
            result += value ? "1" : "0";
        }
        return result;
    }

    private bool[] StringToBoolArray(string data)
    {
        bool[] result = new bool[TOTAL_LEVELS];

        for (int i = 0; i < Mathf.Min(data.Length, TOTAL_LEVELS); i++)
        {
            result[i] = data[i] == '1';
        }

        return result;
    }
}