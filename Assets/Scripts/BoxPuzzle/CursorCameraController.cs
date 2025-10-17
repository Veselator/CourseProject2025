using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorCameraController : MonoBehaviour, ICameraTracker
{
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = Camera.main.transform.position;
    }

    private Vector3 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public Vector3 GetCurrentPosition(Vector3 targetPosition)
    {
        return GetMousePosition();
    }
}
