using UnityEngine;

public class CameraController : MonoBehaviour
{
    private ICameraTracker _tracker;
    [SerializeField] private Transform _target;
    private Transform _defaultTracker;
    public Transform Target
    {
        get => _target; 
        set 
        { 
            _target = value; 
        }
    }

    public static bool IsAbleToUpdate = true;
    [SerializeField] private Vector3 _defaultTrackingPosition = Vector3.zero;

    // Ссылка на Shaker (если есть)
    [SerializeField] private CameraShake _cameraShake;

    private void Start()
    {
        if (PlayerMovementHandler.Instance != null)
            _target = PlayerMovementHandler.Instance.transform;

        _tracker = GetComponent<ICameraTracker>();
        _defaultTracker = _target;

        // Ищем CameraShake в дочерних объектах
        if (_cameraShake == null) _cameraShake = GetComponentInChildren<CameraShake>();
    }

    public void ResetTrackingObject()
    {
        // Для кат-сцен
        _target = _defaultTracker;
    }

    private void LateUpdate()
    {
        if (GlobalFlags.GetFlag(Flags.GameOver)) return;
        if (!IsAbleToUpdate) return;

        // Обновляем позицию
        if (_target != null)
            transform.position = _tracker.GetCurrentPosition(_target.position);
        else
            transform.position = _tracker.GetCurrentPosition(_defaultTrackingPosition);

        // ВАЖНО: применяем тряску ПОСЛЕ обновления позиции
        if (_cameraShake != null)
        {
            ApplyCameraShake();
        }
    }

    private void ApplyCameraShake()
    {
        // Получаем offset тряски и применяем его к позиции
        Vector3 shakeOffset = _cameraShake.ShakeOffset;
        //Quaternion shakeRotation = _cameraShake.ShakeRotation;

        transform.position += shakeOffset;
        //transform.rotation = shakeRotation * transform.rotation;
    }
}