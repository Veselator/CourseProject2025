using UnityEngine;

public interface ICameraTracker
{
    // ��������� ��� �������� ������
    abstract Vector3 GetCurrentPosition(Vector3 targetPosition);
}
