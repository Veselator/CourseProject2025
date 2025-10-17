using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSelectionManager : MonoBehaviour
{
    // Constants
    private const float PREVIEW_LINE_WIDTH = 0.2f;
    private const float CONNECTION_LINE_WIDTH = 0.25f;
    private const float COLLISION_RADIUS = 0.01f;
    private const int MOUSE_BUTTON_LEFT = 0;
    private const float DELAY_AFTER_END_OF_SUCCESS_ANIMATION = 1f;
    private const string LINE_SORTING_LAYER = "TheBackground";
    private const float LINE_ANIMATION_DURATION = 0.2f;

    // Components
    private LineRenderer previewLine;
    private LineRenderer connectionsLine;
    private Camera mainCamera;

    // State
    private BlockSelectionState currentState = BlockSelectionState.Idle;
    private BoxPiece selectedBlock;
    private List<BoxPiece> connectedBlocks = new List<BoxPiece>();
    private List<Vector3> connectionPoints = new List<Vector3>();

    // Animation
    private Coroutine animationCoroutine;

    // Events
    public event Action OnSuccessAnimation;
    public event Action OnFailureAnimation;
    public event Action OnResetSelection;
    public event Action<BoxPiece> OnBlockSelected;

    // Other
    [SerializeField] private LayerMask _blockLayerMask;
    public static BlockSelectionManager Instance { get; private set; }
    [SerializeField] private Color _previewLineColor = new Color(1f, 1f, 1f, 0.5f);
    [SerializeField] private Color _connectedLineColor = Color.blue;
    [SerializeField] private GameObject _failMarkPrefab;

    private Coroutine lineAnimationCoroutine;

    private enum BlockSelectionState
    {
        Idle,
        BlockSelected,
        Connecting,
        CheckingSequence,
        ShowingResult
    }

    private void Awake()
    {
        mainCamera = Camera.main;
        if (Instance == null) Instance = this;
        InitializeLineRenderers();
    }

    private void InitializeLineRenderers()
    {
        // Preview line (from block to cursor)
        GameObject previewObject = new GameObject("PreviewLine");
        previewObject.transform.SetParent(transform);
        previewLine = previewObject.AddComponent<LineRenderer>();
        ConfigureLineRenderer(previewLine, PREVIEW_LINE_WIDTH, _previewLineColor);

        // Connections line (between blocks)
        GameObject connectionsObject = new GameObject("ConnectionsLine");
        connectionsObject.transform.SetParent(transform);
        connectionsLine = connectionsObject.AddComponent<LineRenderer>();
        ConfigureLineRenderer(connectionsLine, CONNECTION_LINE_WIDTH, _connectedLineColor);
    }

    private void ResetLineColors()
    {
        previewLine.startColor = _previewLineColor;
        previewLine.endColor = _previewLineColor;

        connectionsLine.startColor = _connectedLineColor;
        connectionsLine.endColor = _connectedLineColor;
    }

    private void ConfigureLineRenderer(LineRenderer line, float width, Color color)
    {
        line.startWidth = width;
        line.endWidth = width;
        line.startColor = color;
        line.endColor = color;
        line.useWorldSpace = true;
        line.positionCount = 0;

        line.sortingLayerName = LINE_SORTING_LAYER;

        // Add material for better visuals
        line.material = new Material(Shader.Find("Sprites/Default"));
    }

    private void Update()
    {
        HandleInput();
        UpdatePreviewLine();
    }

    private void HandleInput()
    {
        if (currentState == BlockSelectionState.ShowingResult ||
            currentState == BlockSelectionState.CheckingSequence)
            return;

        if (Input.GetMouseButtonDown(MOUSE_BUTTON_LEFT))
        {
            OnMouseDown();
        }
        else if (Input.GetMouseButtonUp(MOUSE_BUTTON_LEFT) && currentState == BlockSelectionState.Connecting)
        {
            OnMouseUp();
        }
    }

    private void OnMouseDown()
    {
        Vector3 mousePos = GetMouseWorldPosition();
        Collider2D hit = Physics2D.OverlapCircle(mousePos, COLLISION_RADIUS, _blockLayerMask);

        if (hit != null)
        {
            BoxPiece piece = hit.GetComponent<BoxPiece>();
            if (piece != null)
            {
                SelectBlock(piece);
                currentState = BlockSelectionState.Connecting;
            }
        }
    }

    private void OnMouseUp()
    {
        Vector3 mousePos = GetMouseWorldPosition();
        Collider2D hit = Physics2D.OverlapCircle(mousePos, COLLISION_RADIUS, _blockLayerMask);

        Debug.Log($"Trying to process OnMouseUp selectedBlock.id = {selectedBlock.Id}");

        if (hit != null && hit.GetComponent<BoxPiece>() != null)
        {
            BoxPiece targetPiece = hit.GetComponent<BoxPiece>();

            Debug.Log($"Trying to process OnMouseUp selectedBlock.id = {selectedBlock.Id} targetPiece.Id = {targetPiece.Id}");
            Debug.Log($"Connected blocks = {connectedBlocks}");
            if (targetPiece != selectedBlock && !connectedBlocks.Contains(targetPiece))
            {
                ConnectBlocks(selectedBlock, targetPiece);
            }
        }

        // Deselect current block
        DeselectCurrentBlock();
        currentState = BlockSelectionState.Idle;
    }

    private void SelectBlock(BoxPiece piece)
    {
        selectedBlock = piece;
        currentState = BlockSelectionState.BlockSelected;

        // Highlight the selected block
        BoxPuzzleEventManager.ShowSelected(piece.GetComponent<Collider2D>());

        // Start showing preview line
        previewLine.positionCount = 2;
    }

    private void DeselectCurrentBlock()
    {
        selectedBlock = null;
        previewLine.positionCount = 0;
    }

    private void ConnectBlocks(BoxPiece from, BoxPiece to)
    {
        // Check if this is the first block
        if (connectedBlocks.Count == 0)
        {
            connectedBlocks.Add(from);
            connectionPoints.Add(from.transform.position);
            BoxPuzzleEventManager.OnRigthSelected();
        }

        // Check if connection is valid
        bool isValid = CheckConnection(from, to);

        if (isValid)
        {
            connectedBlocks.Add(to);
            connectionPoints.Add(to.transform.position);
            UpdateConnectionLine();

            BoxPuzzleEventManager.ShowSelected(to.GetComponent<Collider2D>());
            BoxPuzzleEventManager.OnRigthSelected();

            // Check if all blocks are connected
            CheckForCompletion();
        }
        else
        {
            // Wrong connection - show failure animation
            Debug.Log("Wrong connection attempted.");
            ShowFailureAnimation(from, to);
        }
    }

    private bool CheckConnection(BoxPiece from, BoxPiece to)
    {
        // Use existing logic but with proper null checks
        if (connectedBlocks.Count == 0) return true;
        BoxPiece lastConnected = connectedBlocks[connectedBlocks.Count - 1];
        bool result = to.Id - lastConnected.Id == 1 && to.Id - from.Id == 1;
        Debug.Log($"Checking connection from {from.Id} to {to.Id}. lastConnected.id = {lastConnected.Id} result is {(result ? "TRUE" : "FALSE")}");
        return result;
    }

    private void UpdatePreviewLine()
    {
        if (currentState != BlockSelectionState.Connecting || selectedBlock == null)
        {
            previewLine.positionCount = 0;
            return;
        }

        Vector3 startPos = selectedBlock.transform.position;
        Vector3 endPos = GetMouseWorldPosition();

        previewLine.SetPosition(0, startPos);
        previewLine.SetPosition(1, endPos);
    }

    private void UpdateConnectionLine()
    {
        if (lineAnimationCoroutine != null)
        {
            StopCoroutine(lineAnimationCoroutine);
        }
        lineAnimationCoroutine = StartCoroutine(AnimateLineToNewPoint());
    }

    private IEnumerator AnimateLineToNewPoint()
    {
        if (connectionPoints.Count < 2)
        {
            // Если только одна точка, просто отображаем её
            connectionsLine.positionCount = connectionPoints.Count;
            connectionsLine.SetPositions(connectionPoints.ToArray());
            yield break;
        }

        Vector3 startPoint = connectionPoints[connectionPoints.Count - 2];
        Vector3 endPoint = connectionPoints[connectionPoints.Count - 1];

        // Устанавливаем количество точек (все предыдущие + одна промежуточная для анимации)
        connectionsLine.positionCount = connectionPoints.Count;

        float elapsed = 0f;

        while (elapsed < LINE_ANIMATION_DURATION)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / LINE_ANIMATION_DURATION);

            // Интерполируем позицию последней точки
            Vector3 currentPoint = Vector3.Lerp(startPoint, endPoint, t * t);

            // Обновляем все точки линии
            for (int i = 0; i < connectionPoints.Count - 1; i++)
            {
                connectionsLine.SetPosition(i, connectionPoints[i]);
            }

            // Последняя точка анимируется
            connectionsLine.SetPosition(connectionPoints.Count - 1, currentPoint);

            yield return null;
        }

        // Убеждаемся, что линия точно достигла конечной точки
        connectionsLine.SetPosition(connectionPoints.Count - 1, endPoint);
    }

    private void CheckForCompletion()
    {
        // This will be called from BoxPuzzleGameManager when needed
        // For now, just check if we have enough blocks
        if (LevelManager.Instance != null && connectedBlocks.Count >= LevelManager.Instance.CurrentLevelPieceCount)
        {
            ShowSuccessAnimation();
        }
    }

    private void ShowSuccessAnimation()
    {
        if (animationCoroutine != null) StopCoroutine(animationCoroutine);
        animationCoroutine = StartCoroutine(SuccessAnimationCoroutine());
        OnSuccessAnimation?.Invoke();
    }

    private void ShowFailureAnimation(BoxPiece from, BoxPiece to)
    {
        if (animationCoroutine != null) StopCoroutine(animationCoroutine);
        animationCoroutine = StartCoroutine(FailureAnimationCoroutine());
        SpawnFailMark(GetMiddlePoint(from.gameObject.transform.position, to.gameObject.transform.position));
        OnFailureAnimation?.Invoke();
    }

    private Vector3 GetMiddlePoint(Vector3 firstPoint, Vector3 secondPoint)
    {
        return (firstPoint + secondPoint) / 2f;
    }

    private void SpawnFailMark(Vector3 position)
    {
        Instantiate(_failMarkPrefab, position, Quaternion.identity);
    }
        
    // Корутина для connectionsLine для уменьшения толщины от текущей до 0 по ease in
    private IEnumerator HideLine(float duration) 
    {
        float elapsed = 0f;
        float startWidth = connectionsLine.startWidth;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            float easedT = t * t; // ease in
            float currentWidth = Mathf.Lerp(startWidth, 0f, easedT);

            connectionsLine.startWidth = currentWidth;
            connectionsLine.endWidth = currentWidth;
            yield return null;
        }

        connectionsLine.positionCount = 0;
        connectionsLine.startWidth = startWidth;
        connectionsLine.endWidth = startWidth;
    }


    private IEnumerator SuccessAnimationCoroutine()
    {
        currentState = BlockSelectionState.ShowingResult;

        // Animate line color to green
        float duration = 1f;
        float elapsed = 0f;
        Color startColor = connectionsLine.startColor;
        Color successColor = Color.green;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            Color currentColor = Color.Lerp(startColor, successColor, t);
            connectionsLine.startColor = currentColor;
            connectionsLine.endColor = currentColor;
            yield return null;
        }

        StartCoroutine(HideLine(0.5f));
        yield return new WaitForSeconds(DELAY_AFTER_END_OF_SUCCESS_ANIMATION);
        
        // Proceed to next level
        ResetLineColors();
        BoxPuzzleEventManager.LevelChange();
        ResetSelection();
    }

    private IEnumerator FailureAnimationCoroutine()
    {
        currentState = BlockSelectionState.ShowingResult;

        // Flash red
        Color originalColor = connectionsLine.startColor;
        for (int i = 0; i < 3; i++)
        {
            connectionsLine.startColor = Color.red;
            connectionsLine.endColor = Color.red;
            yield return new WaitForSeconds(0.2f);
            connectionsLine.startColor = originalColor;
            connectionsLine.endColor = originalColor;
            yield return new WaitForSeconds(0.2f);
        }

        // Reset everything
        BoxPuzzleEventManager.RetrunToNormOp();
        ResetSelection();
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        return mousePos;
    }

    private void ResetSelection()
    {
        connectedBlocks.Clear();
        connectionPoints.Clear();
        connectionsLine.positionCount = 0;
        previewLine.positionCount = 0;
        selectedBlock = null;
        currentState = BlockSelectionState.Idle;

        OnResetSelection?.Invoke();
    }

    public void ResetLine()
    {
        ResetSelection();
    }

    private void OnEnable()
    {
        BoxPuzzleEventManager.OnLevelChange += ResetLine;
    }

    private void OnDisable()
    {
        BoxPuzzleEventManager.OnLevelChange -= ResetLine;
    }
}