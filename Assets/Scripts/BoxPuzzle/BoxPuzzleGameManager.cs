using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPuzzleGameManager : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager; // Изменено с AddPiecesToGame
    private BlockSelectionManager selectionManager;

    private int currentLevelPieces = 0;
    private int totalCurrentLevelPieces;

    private void Awake()
    {
        // Находим менеджеры если не назначены
        if (levelManager == null)
            levelManager = FindObjectOfType<LevelManager>();

        selectionManager = FindObjectOfType<BlockSelectionManager>();
    }

    private void Start()
    {
        UpdateLevelInfo();
    }

    private void UpdateLevelInfo()
    {
        if (levelManager != null)
        {
            totalCurrentLevelPieces = levelManager.CurrentLevelPieceCount;
            Debug.Log($"Level {levelManager.CurrentLevel}/{levelManager.TotalLevels} - Pieces: {totalCurrentLevelPieces}");
        }
    }

    private void IncrementCurrentLevelPieces()
    {
        currentLevelPieces++;
        CheckForCompletion();
    }

    private void ResetCurrentPieces()
    {
        currentLevelPieces = 0;
    }

    private void CheckForCompletion()
    {
        if (currentLevelPieces >= totalCurrentLevelPieces && totalCurrentLevelPieces > 0)
        {
            Debug.Log("Level completed!");
            StartCoroutine(CompleteLevelSequence());
        }
    }

    private IEnumerator CompleteLevelSequence()
    {
        // Ждем анимацию успеха
        yield return new WaitForSeconds(1.5f);

        // Переход на следующий уровень
        BoxPuzzleEventManager.LevelChange();
        currentLevelPieces = 0;
        UpdateLevelInfo();
    }

    private void OnLevelChange()
    {
        ResetCurrentPieces();
        UpdateLevelInfo();
    }

    private void OnEnable()
    {
        BoxPuzzleEventManager.OnRigthSelected += IncrementCurrentLevelPieces;
        BoxPuzzleEventManager.OnReturnToNormalOpacity += ResetCurrentPieces;
        BoxPuzzleEventManager.OnLevelChange += OnLevelChange;
    }

    private void OnDisable()
    {
        BoxPuzzleEventManager.OnRigthSelected -= IncrementCurrentLevelPieces;
        BoxPuzzleEventManager.OnReturnToNormalOpacity -= ResetCurrentPieces;
        BoxPuzzleEventManager.OnLevelChange -= OnLevelChange;
    }
}