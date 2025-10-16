using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject[] levelPrefabs;
    [SerializeField] private float pieceSpacing = 2f;

    private int currentLevelIndex = 0;
    private GameObject currentLevelInstance;
    private List<BoxPiece> currentLevelPieces = new List<BoxPiece>();

    public int CurrentLevelPieceCount => currentLevelPieces.Count;
    public int TotalLevels => levelPrefabs.Length;
    public int CurrentLevel => currentLevelIndex + 1;

    public static LevelManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        LoadLevel();
    }

    private void LoadLevel()
    {
        if (currentLevelIndex >= levelPrefabs.Length)
        {
            Debug.Log("?? All levels completed!");
            OnAllLevelsCompleted();
            return;
        }

        // Clear previous level if exists
        if (currentLevelInstance != null)
        {
            Destroy(currentLevelInstance);
            currentLevelPieces.Clear();
        }

        // Instantiate new level
        currentLevelInstance = Instantiate(levelPrefabs[currentLevelIndex]);
        SetupLevelPieces();

        Debug.Log($"Level {CurrentLevel} loaded with {CurrentLevelPieceCount} pieces");
    }

    private void SetupLevelPieces()
    {
        Transform[] children = currentLevelInstance.GetComponentsInChildren<Transform>();

        int pieceIndex = 0;
        foreach (Transform child in children)
        {
            if (child == currentLevelInstance.transform) continue;

            // Setup collider
            if (child.GetComponent<Collider2D>() == null)
            {
                child.gameObject.AddComponent<BoxCollider2D>();
            }

            // Setup BoxPiece component
            BoxPiece piece = child.GetComponent<BoxPiece>();
            if (piece == null)
            {
                piece = child.gameObject.AddComponent<BoxPiece>();
            }

            // Try to parse ID from name, otherwise use sequential numbering
            if (!int.TryParse(child.name, out int id))
            {
                id = pieceIndex + 1;
            }

            piece.Id = id;
            piece.Initialize(pieceIndex);
            currentLevelPieces.Add(piece);

            pieceIndex++;
        }
    }

    public void NextLevel()
    {
        currentLevelIndex++;
        LoadLevel();
    }

    private void OnAllLevelsCompleted()
    {
        // Show completion UI or restart from first level
        Debug.Log("Game completed! Restarting...");
        currentLevelIndex = 0;
        LoadLevel();
    }

    private void HandleLevelChange()
    {
        NextLevel();
    }

    private void OnEnable()
    {
        BoxPuzzleEventManager.OnLevelChange += HandleLevelChange;
    }

    private void OnDisable()
    {
        BoxPuzzleEventManager.OnLevelChange -= HandleLevelChange;
    }
}