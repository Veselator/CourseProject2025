using UnityEngine;

public class CameraZoomByMouse : MonoBehaviour
{
    [Header("Camera Reference")]
    [SerializeField] private Camera _targetCamera;

    [Header("Zoom Settings")]
    [SerializeField] private float _minOrthographicSize = 3f;
    [SerializeField] private float _maxOrthographicSize = 8f;
    [SerializeField] private float _zoomSmoothSpeed = 5f;

    [Header("Distance Settings")]
    [SerializeField] private float _maxDistanceFromCenter = 500f;
    [SerializeField] private AnimationCurve _zoomCurve = AnimationCurve.Linear(0, 0, 1, 1);

    private float _targetSize;

    private void Awake()
    {
        if (_targetCamera == null)
        {
            _targetCamera = Camera.main;
        }

        _targetSize = _targetCamera.orthographicSize;
    }

    private void Update()
    {
        Vector2 screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        Vector2 mousePosition = Input.mousePosition;

        float distanceFromCenter = Vector2.Distance(mousePosition, screenCenter);

        float normalizedDistance = Mathf.Clamp01(distanceFromCenter / _maxDistanceFromCenter);

        float curveValue = _zoomCurve.Evaluate(normalizedDistance);

        _targetSize = Mathf.Lerp(_minOrthographicSize, _maxOrthographicSize, curveValue);

        _targetCamera.orthographicSize = Mathf.Lerp(
            _targetCamera.orthographicSize,
            _targetSize,
            Time.deltaTime * _zoomSmoothSpeed
        );
    }
}