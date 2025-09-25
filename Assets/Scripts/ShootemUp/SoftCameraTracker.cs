using UnityEngine;

public class SoftCameraTracker : MonoBehaviour, ICameraTracker
{
    // ������ - ���������, �� ������������� �������� � �������� ������� ����
    private Vector3 startPosition;
    private readonly float blendFactor = 0.05f;

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
        return Vector3.Lerp(startPosition, GetMousePosition(), blendFactor);
    }
}
