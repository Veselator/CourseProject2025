using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGameManager : MonoBehaviour
{
    // Отвечает за загрузку уровней

    private int currentLevel = 0;
    private int lastLevelIndex = 0;
    public int CurrentLevel => currentLevel;

    [SerializeField] private GameObject[] levelsGO;

    public static QuestGameManager Instance { get; private set; }
    public delegate void LevelLoaded(int levelID);
    public event LevelLoaded OnLevelLoaded;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        lastLevelIndex = levelsGO.Length - 1;
    }

    public void NextLevel()
    {
        HideLevel(currentLevel);
        currentLevel++;

        if (currentLevel > lastLevelIndex)
        {
            // Мы победили - ура
            return;
        }

        LoadLevel(currentLevel);
    }

    private void HideLevel(int levelId)
    {
        levelsGO[levelId].SetActive(false);
    }

    private void LoadLevel(int levelId)
    {
        levelsGO[levelId].SetActive(true);
        OnLevelLoaded?.Invoke(levelId);
    }
}
