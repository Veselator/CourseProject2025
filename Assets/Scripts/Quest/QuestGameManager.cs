using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestGameManager : MonoBehaviour
{
    // �������� �� �������� �������

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

        InitLevels();
    }

    private void InitLevels()
    {
        for (int i = 1; i < levelsGO.Length; i++)
        {
            HideLevel(i);
        }
    }

    public void NextLevel()
    {
        HideLevel(currentLevel);
        currentLevel++;

        if (currentLevel == levelsGO.Count())
        {
            // ���� ����� �� ������� ������ ������� - ������, �� ��������
            GlobalFlags.SetFlag(Flags.GameWin);
            return;
        }

        if (currentLevel > lastLevelIndex)
        {
            // �� �������� - ���
            return;
        }

        LoadLevel(currentLevel);
    }

    private void HideLevel(int levelId)
    {
        levelsGO[levelId].SetActive(false);
    }

    // �������� ������ ������
    private void LoadLevel(int levelId)
    {
        levelsGO[levelId].SetActive(true);
        OnLevelLoaded?.Invoke(levelId);
    }
}
