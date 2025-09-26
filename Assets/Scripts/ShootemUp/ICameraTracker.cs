using UnityEngine;

public interface ICameraTracker
{
    // Интерфейс для движения камеры
    abstract Vector3 GetCurrentPosition(Vector3 targetPosition);
}
