using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class LineCreator : MonoBehaviour
{
    // Constants
    private const float LINE_WIDTH = 0.1f;
    private const float COLLISION_RADIUS = 0.01f;
    private const int MOUSE_BUTTON_LEFT = 0;

    // Components
    private LineRenderer line;
    private Camera mainCamera;

    // State
    private bool isDrawing;
    private List<Vector3> linePoints;
    private Collider2D currentHit;
    private Collider2D lastHit;

    private void Awake()
    {
        InitializeLineRenderer();
        mainCamera = Camera.main;
        linePoints = new List<Vector3>();
    }

    private void InitializeLineRenderer()
    {
        line = GetComponent<LineRenderer>();
        if (line == null)
            line = gameObject.AddComponent<LineRenderer>();

        line.startWidth = LINE_WIDTH;
        line.endWidth = LINE_WIDTH;
        line.startColor = Color.red;
        line.endColor = Color.blue;
        line.useWorldSpace = true;
        line.positionCount = 0;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(MOUSE_BUTTON_LEFT) && !isDrawing)
        {
            StartDrawing();
        }

        if (isDrawing)
        {
            UpdateDrawing();

            if (Input.GetMouseButtonUp(MOUSE_BUTTON_LEFT))
            {
                StopDrawing();
            }
        }
    }

    private void StartDrawing()
    {
        Vector3 mousePos = GetMouseWorldPosition();

        ResetLineState();
        isDrawing = true;

        // Check initial collision
        currentHit = Physics2D.OverlapCircle(mousePos, COLLISION_RADIUS);

        if (currentHit)
        {
            BoxPuzzleEventManager.ShowSelected(currentHit);
            BoxPuzzleEventManager.OnRigthSelected();
        }

        // Add first point
        linePoints.Add(mousePos);
        UpdateLineRenderer();
    }

    private void UpdateDrawing()
    {
        Vector3 mousePos = GetMouseWorldPosition();

        // Add new point to line
        linePoints.Add(mousePos);
        UpdateLineRenderer();

        // Check for collisions
        Collider2D hit = Physics2D.OverlapCircle(mousePos, COLLISION_RADIUS);

        if (hit && hit != currentHit)
        {
            HandleNewCollision(hit);
        }
    }

    private void HandleNewCollision(Collider2D hit)
    {
        lastHit = currentHit;
        currentHit = hit;

        BoxPuzzleEventManager.ShowSelected(currentHit);

        // Check if selection is correct
        bool isCorrect = CheckCorrection.Check(lastHit, currentHit);

        if (!isCorrect)
        {
            StopDrawing(resetVisuals: true);
        }
        else
        {
            BoxPuzzleEventManager.OnRigthSelected();
        }
    }

    private void StopDrawing(bool resetVisuals = true)
    {
        if (resetVisuals)
        {
            BoxPuzzleEventManager.RetrunToNormOp();
        }

        ResetLineState();
    }

    private void UpdateLineRenderer()
    {
        line.positionCount = linePoints.Count;
        line.SetPositions(linePoints.ToArray());
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        return mousePos;
    }

    private void ResetLineState()
    {
        isDrawing = false;
        linePoints.Clear();
        line.positionCount = 0;
        currentHit = null;
        lastHit = null;
    }

    public void ResetLine()
    {
        ResetLineState();
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