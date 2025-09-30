using System.Collections;
using UnityEngine;

public class Camera_Movement : MonoBehaviour
{
    public static Camera_Movement Instance { get; private set; }

    [Header("Target Settings")]
    [SerializeField] private Transform player;

    [Header("Camera Settings")]
    public Vector3 offset = new Vector3(0, 5, -10);
    [SerializeField] private float smoothSpeed = 5f;

    [Header("Camera Bounds (Optional)")]
    [SerializeField] private bool useBounds = false;
    [SerializeField] private Vector2 minBounds;
    [SerializeField] private Vector2 maxBounds;

    private Vector3 velocity = Vector3.zero;
    private Camera cam;

    private void Awake()
    {
        InitializeSingleton();
        CacheComponents();
        FindPlayer();
    }

    private void InitializeSingleton()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void CacheComponents()
    {
        cam = GetComponent<Camera>();

        if (cam == null)
        {
            Debug.LogError("Camera component is missing on Camera_Movement object!");
        }
    }

    private void FindPlayer()
    {
        // Если player не назначен в Inspector, ищем по тегу
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject.transform;
            }
            else
            {
                Debug.LogWarning("Player not found! Assign player manually or add 'Player' tag.");
            }
        }
    }

    private void LateUpdate()
    {
        // LateUpdate вызывается после всех Update, поэтому камера следует за актуальной позицией игрока
        Move_Camera_To_Player();
    }

    private void Move_Camera_To_Player()
    {
        if (player == null)
        {
            return;
        }

        Vector3 targetPosition = player.position + offset;

        // Ограничиваем позицию камеры, если включены границы
        if (useBounds)
        {
            targetPosition = ApplyBounds(targetPosition);
        }

        // Плавное следование за игроком
        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref velocity,
            1f / smoothSpeed
        );
    }

    private Vector3 ApplyBounds(Vector3 targetPosition)
    {
        float clampedX = Mathf.Clamp(targetPosition.x, minBounds.x, maxBounds.x);
        float clampedY = Mathf.Clamp(targetPosition.y, minBounds.y, maxBounds.y);

        return new Vector3(clampedX, clampedY, targetPosition.z);
    }

    // Публичные методы для внешнего управления
    public void SetPlayer(Transform newPlayer)
    {
        player = newPlayer;
    }

    public void SetOffset(Vector3 newOffset)
    {
        offset = newOffset;
    }

    public void SetSmoothSpeed(float newSpeed)
    {
        smoothSpeed = Mathf.Max(0.1f, newSpeed);
    }

    public void ShakeCamera(float duration, float magnitude)
    {
        StartCoroutine(CameraShakeCoroutine(duration, magnitude));
    }

    private IEnumerator CameraShakeCoroutine(float duration, float magnitude)
    {
        Vector3 originalOffset = offset;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            offset = originalOffset + new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        offset = originalOffset;
    }

    // Визуализация границ камеры в редакторе
    private void OnDrawGizmosSelected()
    {
        if (!useBounds) return;

        Gizmos.color = Color.yellow;

        Vector3 bottomLeft = new Vector3(minBounds.x, minBounds.y, transform.position.z);
        Vector3 topRight = new Vector3(maxBounds.x, maxBounds.y, transform.position.z);
        Vector3 topLeft = new Vector3(minBounds.x, maxBounds.y, transform.position.z);
        Vector3 bottomRight = new Vector3(maxBounds.x, minBounds.y, transform.position.z);

        Gizmos.DrawLine(bottomLeft, topLeft);
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
    }
}