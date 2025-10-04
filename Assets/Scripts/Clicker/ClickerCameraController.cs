using System.Collections;
using UnityEngine;

public class ClickerCameraController : MonoBehaviour
{
    [SerializeField] private Transform[] cameraPoints;
    [SerializeField] private float smoothTime = 0.5f;
    [SerializeField] private float positionThreshold = 0.01f;

    private int currentCameraPosition = 0;
    private Coroutine movementCoroutine;

    private void Start()
    {
        if (cameraPoints.Length > 0)
        {
            transform.position = cameraPoints[0].position;
        }
    }

    public void ChangeCameraPosition(int newPos)
    {
        if (newPos < 0 || newPos >= cameraPoints.Length) return;
        if (newPos == currentCameraPosition) return;

        currentCameraPosition = newPos;

        // Останавливаем предыдущую корутину, если она выполняется
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
        }

        movementCoroutine = StartCoroutine(MoveCameraCoroutine(cameraPoints[newPos].position));
    }

    private IEnumerator MoveCameraCoroutine(Vector3 targetPosition)
    {
        Vector3 velocity = Vector3.zero;

        while (Vector3.Distance(transform.position, targetPosition) > positionThreshold)
        {
            transform.position = Vector3.SmoothDamp(
                transform.position,
                targetPosition,
                ref velocity,
                smoothTime
            );

            yield return null;
        }

        // Гарантируем точную позицию
        transform.position = targetPosition;
        movementCoroutine = null;
    }
}