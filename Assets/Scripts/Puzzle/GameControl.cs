using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    [Header("Drag Settings")]
    [SerializeField] private float dragSmoothness = 50f;
    [SerializeField] private float pickupDetectionRadius = 0.01f;

    private Collider2D hit = null;
    private LayerMask puzzleLayer;
    private int sortingLevel = 0;
    private Vector2 offset;
    private Vector2 mousePosCashed;

    // Кэшированные компоненты для оптимизации
    private Camera mainCamera;
    private PuzzlePiece currentPuzzlePiece;
    private PuzzlePieceVisual currentVisual;
    private SpriteRenderer currentSpriteRenderer;

    private void Awake()
    {
        InitializeReferences();
    }

    private void Update()
    {
        Control();
    }

    private void InitializeReferences()
    {
        puzzleLayer = LayerMask.GetMask("Puzzle");
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found!");
        }
    }

    private void Control()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseDown();
        }

        if (Input.GetMouseButtonUp(0))
        {
            HandleMouseUp();
        }

        if (hit != null)
        {
            HandleDragging();
        }
    }

    private void HandleMouseDown()
    {
        mousePosCashed = GetMouseWorldPosition();
        Vector3 worldMousePos = mousePosCashed;
        worldMousePos.z = 0f;

        hit = Physics2D.OverlapCircle(worldMousePos, pickupDetectionRadius, puzzleLayer);

        if (hit != null)
        {
            OnPiecePickedUp(hit, worldMousePos);
        }
    }

    private void HandleMouseUp()
    {
        if (hit != null)
        {
            OnPieceReleased();
        }

        ClearCurrentPiece();
    }

    private void HandleDragging()
    {
        mousePosCashed = GetMouseWorldPosition();
        Vector3 targetPos = mousePosCashed + offset;
        targetPos.z = 0f;

        hit.transform.position = Vector3.Lerp(
            hit.transform.position,
            targetPos,
            dragSmoothness * Time.deltaTime
        );
    }

    private void OnPiecePickedUp(Collider2D pieceCollider, Vector3 worldMousePos)
    {
        // Кэшируем компоненты
        CacheComponents(pieceCollider);

        // Увеличиваем sorting order
        IncreaseSortingLevel();

        // Вычисляем offset для плавного перетаскивания
        offset = (Vector2)(pieceCollider.transform.position - worldMousePos);

        // Запускаем анимацию подбора (вызывается только один раз при подборе)
        if (currentVisual != null)
        {
            currentVisual.StartScaling();
        }
    }

    private void OnPieceReleased()
    {
        if (currentPuzzlePiece != null)
        {
            currentPuzzlePiece.CheckCorrect();
        }
    }

    private void CacheComponents(Collider2D pieceCollider)
    {
        currentPuzzlePiece = pieceCollider.GetComponent<PuzzlePiece>();
        currentVisual = pieceCollider.GetComponent<PuzzlePieceVisual>();
        currentSpriteRenderer = pieceCollider.GetComponent<SpriteRenderer>();

        // Проверка наличия компонентов
        if (currentPuzzlePiece == null)
        {
            Debug.LogWarning($"PuzzlePiece component missing on {pieceCollider.name}");
        }

        if (currentVisual == null)
        {
            Debug.LogWarning($"PuzzlePieceVisual component missing on {pieceCollider.name}");
        }
    }

    private void ClearCurrentPiece()
    {
        hit = null;
        currentPuzzlePiece = null;
        currentVisual = null;
        currentSpriteRenderer = null;
    }

    private Vector2 GetMouseWorldPosition()
    {
        if (mainCamera == null) return Vector2.zero;

        Vector3 worldMousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        worldMousePos.z = 0f;
        return worldMousePos;
    }

    private void IncreaseSortingLevel()
    {
        if (currentSpriteRenderer == null) return;

        sortingLevel = (sortingLevel >= 100) ? 0 : sortingLevel;
        currentSpriteRenderer.sortingOrder = sortingLevel++;
    }
}