using System;
using System.Collections.Generic;
using UnityEngine;

public class Gm : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private Transform piece;
    [SerializeField] private Transform border;

    public Vector3 BorderPosition => border.position;

    [Header("Puzzle Settings")]
    [SerializeField] private Texture2D[] pic;
    [SerializeField] private Transform gameHolder;
    [Range(2, 6)]
    [SerializeField] private int startDifficulty;

    [Header("Fixed Puzzle Size")]
    [SerializeField] private float puzzleWorldSize = 0.2f; // Фиксированный размер паззла в мировых координатах
    public float PuzzleWorldSize => puzzleWorldSize;
    [SerializeField] private float _spriteSize = 100f; // Pixels per unit для спрайтов
    [SerializeField] private float _puzzleGapOffsetY = -0.2f;
    [SerializeField] private float _puzzleOffsetX = -0.2f;
    [SerializeField] private int[] currentDifficulty;


    const int MaxLevel = 3;
    const int MaxDifficulty = 6;

    private int difficulty = 0;
    private int picCounter = 0;

    public Texture2D CurrentTexture => pic[picCounter];

    // Параметры паззла
    private Vector2Int gridSize; // Количество кусков по X и Y
    public Vector2Int GridSize => gridSize;
    private Vector2 pieceSize;   // Размер одного куска в мировых координатах
    public Vector2 PieceSize => pieceSize;
    private int puzzlesSolved = 0;

    private List<PuzzlePiece> pieces = new List<PuzzlePiece>();
    private int correctCounter = 0;

    public static Gm Instance { get; private set; }
    public event Action OnPuzzleCompleted;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        // ВАЖНО: обновляем gameHolder если нужно
        UpdateGameHolder(); // <-- Добавить обратно этот вызов

        // Создаём паззл
        CreatePuzzle(pic[picCounter]);

        // Разбрасываем куски
        PuzzleScattering.Instance.ScatterPuzzles(pieces);
    }

    private void CreatePuzzle(Texture2D texture)
    {
        // Очищаем старые куски если есть
        if(pieces.Count > 0) ClearPieces();

        // Вычисляем сетку на основе сложности и соотношения сторон картинки
        gridSize = CalculateGridSize(texture);

        // Вычисляем размер одного куска
        pieceSize = CalculatePieceSize();

        // Создаём куски паззла
        CreatePuzzlePieces(texture);

        // Обновляем рамку
        UpdateBorder();
    }

    private Vector2Int CalculateGridSize(Texture2D texture)
    {
        Vector2Int grid = Vector2Int.zero;
        float aspectRatio = (float)texture.width / texture.height;

        if (aspectRatio >= 1f) // Картинка шире чем выше
        {
            grid.x = currentDifficulty[difficulty];
            grid.y = Mathf.RoundToInt(currentDifficulty[difficulty] / aspectRatio);
            if (grid.y < 2) grid.y = 2; // Минимум 2 куска по высоте
        }
        else // Картинка выше чем шире
        {
            grid.y = currentDifficulty[difficulty];
            grid.x = Mathf.RoundToInt(currentDifficulty[difficulty] * aspectRatio);
            if (grid.x < 2) grid.x = 2; // Минимум 2 куска по ширине
        }

        return grid;
    }

    private Vector2 CalculatePieceSize()
    {
        float pieceWidth = puzzleWorldSize / gridSize.x;

        // Вычисляем высоту исходя из соотношения сторон текстуры
        float aspectRatio = (float)CurrentTexture.width / CurrentTexture.height;
        float totalHeight = puzzleWorldSize / aspectRatio;
        float pieceHeight = totalHeight / gridSize.y;

        return new Vector2(pieceWidth, pieceHeight);
    }

    private void CreatePuzzlePieces(Texture2D texture)
    {
        int texWidth = texture.width;
        int texHeight = texture.height;

        // Размер одного куска в пикселях текстуры
        int pieceWidthPx = texWidth / gridSize.x;
        int pieceHeightPx = texHeight / gridSize.y;

        // Начальная позиция (левый нижний угол паззла)
        Vector2 startPos = new Vector2(
            -(gridSize.x - 1) * pieceSize.x * 0.5f + _puzzleOffsetX,
            -(gridSize.y - 1) * pieceSize.y * 0.5f
        );

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                // Создаём GameObject куска
                Transform currPiece = Instantiate(piece, gameHolder);

                // Вырезаем спрайт из текстуры
                Rect rect = new Rect(
                    x * pieceWidthPx,
                    y * pieceHeightPx,
                    pieceWidthPx,
                    pieceHeightPx
                );

                // Создаём спрайт с правильным Pixels Per Unit
                // ВАЖНО: используем pieceWidthPx как PPU для правильного размера
                float pixelsPerUnit = (pieceWidthPx / pieceSize.x + pieceHeightPx / pieceSize.y) / 2f;
                Sprite sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f), pixelsPerUnit);

                SpriteRenderer sr = currPiece.GetComponent<SpriteRenderer>();
                sr.sprite = sprite;

                // Теперь спрайт уже правильного размера, не нужно масштабировать
                currPiece.localScale = Vector3.one;

                // Обновляем BoxCollider2D под размер спрайта
                BoxCollider2D boxCollider = currPiece.GetComponent<BoxCollider2D>();
                if (boxCollider != null)
                {
                    // Автоматически подстраиваем размер коллайдера под спрайт
                    boxCollider.size = sr.sprite.bounds.size;

                    // Или можно задать вручную
                    // boxCollider.size = new Vector2(pieceSize.x, pieceSize.y);
                }

                // Альтернативный вариант - пересоздать коллайдер
                // Destroy(boxCollider);
                // currPiece.gameObject.AddComponent<BoxCollider2D>();

                // Позиционируем кусок
                Vector3 correctPosition = new Vector3(
                    startPos.x + x * pieceSize.x,
                    startPos.y + y * (pieceSize.y), //  startPos.y + y * (pieceSize.y + _puzzleGapOffsetY) - что-бы не был виден зазор
                    0f
                );
                currPiece.localPosition = correctPosition;
                currPiece.name = $"Piece_{x}_{y}";

                // Настраиваем компонент PuzzlePiece
                PuzzlePiece puzzlePiece = currPiece.GetComponent<PuzzlePiece>();
                puzzlePiece.SetCorrectPos(correctPosition);
                pieces.Add(puzzlePiece);
            }
        }
    }

    private void UpdateBorder()
    {
        if (border == null) return;

        // Если border - это единичный квадрат, то просто масштабируем напрямую
        float borderWidth = gridSize.x * pieceSize.x;
        float borderHeight = gridSize.y * pieceSize.y;

        border.localScale = new Vector3(borderWidth, borderHeight, 1f);
        border.position = new Vector3(_puzzleOffsetX, 0, 0);

        // Добавим немного отступа для рамки (опционально)
        float borderPadding = 1.1f; // 10% больше
        border.localScale *= borderPadding;
    }

    private void UpdateGameHolder()
    {
        // Упрощённая версия - gameHolder теперь не зависит от камеры
        gameHolder.localScale = Vector3.one;
    }

    public void ClearPieces()
    {
        foreach (var p in pieces)
        {
            if (p != null && p.gameObject != null)
                Destroy(p.gameObject);
        }
        pieces.Clear();
        correctCounter = 0;
    }

    private void IncreaseCorrectCounter()
    {
        correctCounter++;
    }

    private void CheckCorrect()
    {
        // Проверка, решили ли мы текущий паззл
        if (correctCounter == pieces.Count)
        {
            puzzlesSolved++;

            // Переход к следующему паззлу
            //GetToNextPuzzle();
            OnPuzzleCompleted?.Invoke();
        }
    }

    private bool IsWon()
    {
        return puzzlesSolved >= MaxLevel;
    }

    public void GetToNextPuzzle()
    {
        if (IsWon())
        {
            Debug.Log("Game Completed! All puzzles solved.");
            // Здесь можно добавить логику окончания игры, например, показать экран победы
            PuzzleWinGame.Instance.WinGame();
            return;
        }
        difficulty++;
        picCounter++;

        // Проверяем, есть ли ещё картинки
        if (picCounter >= pic.Length)
        {
            Debug.LogWarning("No more pictures available!");
            picCounter = 0; // Зацикливаем или можно победить
        }

        // Создаём новый паззл
        Initialize();
    }

    private void OnEnable()
    {
        EventManager.OnPuzzleCorrect += IncreaseCorrectCounter;
        EventManager.OnPuzzleCorrect += CheckCorrect;
    }

    private void OnDisable()
    {
        EventManager.OnPuzzleCorrect -= IncreaseCorrectCounter;
        EventManager.OnPuzzleCorrect -= CheckCorrect;
    }
}