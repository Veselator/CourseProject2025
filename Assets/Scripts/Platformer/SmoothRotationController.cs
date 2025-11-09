using UnityEngine;

public class SmoothRotationController : MonoBehaviour
{
    [Header("Настройка поворота")]
    [SerializeField] private float _maxRotateValue = 15f;
    [SerializeField] private float _interpolationFactorForMovement = 0.02f;
    [SerializeField] private float _interpolationFactorForRotate = 0.05f;
    [SerializeField] private float _interpolationFactorIfNoButtonPressed = 0.05f;

    [SerializeField] private RigidbodyPlatformerMovement _movementController;

    private float _currentTargetRotate = 0f;
    private float _currentRotation = 0f;
    private float _currentLerpFactor = 0f;

    private bool _isAtTargetRotation = true;
    private const float _rotationThreshold = 0.01f;

    private float _smoothDampVelocity;

    private void OnEnable()
    {
        _movementController.OnAnyMove += HandleMovement;
        _movementController.OnRotated += HandleRotation;
        _movementController.OnNoMove += HandleNoMovement;
    }

    private void OnDisable()
    {
        _movementController.OnAnyMove -= HandleMovement;
        _movementController.OnRotated -= HandleRotation;
        _movementController.OnNoMove -= HandleNoMovement;
    }

    private void FixedUpdate()
    {
        if (!_isAtTargetRotation)
        {
            ApplySmoothRotation();
        }
    }

    private void HandleMovement(Vector2 velocity)
    {
        _currentTargetRotate = velocity.x > 0f ? -_maxRotateValue : _maxRotateValue;

        _currentLerpFactor = _interpolationFactorForMovement;
        _isAtTargetRotation = false;
    }

    private void HandleRotation(Vector2 velocity)
    {
        _currentTargetRotate = velocity.x > 0f ? -_maxRotateValue : _maxRotateValue;

        _currentLerpFactor = _interpolationFactorForRotate;
        _isAtTargetRotation = false;
    }

    private void HandleNoMovement()
    {
        _currentTargetRotate = 0f;
        _currentLerpFactor = _interpolationFactorIfNoButtonPressed;
        _isAtTargetRotation = false;
    }

    private void ApplySmoothRotation()
    {
        _currentRotation = Mathf.SmoothDamp(
            _currentRotation,
            _currentTargetRotate,
            ref _smoothDampVelocity,
            _currentLerpFactor
        );

        transform.rotation = Quaternion.Euler(0f, 0f, _currentRotation);

        if (Mathf.Abs(_currentRotation - _currentTargetRotate) < _rotationThreshold)
        {
            _currentRotation = _currentTargetRotate;
            transform.rotation = Quaternion.Euler(0f, 0f, _currentRotation);
            _isAtTargetRotation = true;
        }
    }

    public void ResetRotation()
    {
        _currentRotation = 0f;
        _currentTargetRotate = 0f;
        transform.rotation = Quaternion.identity;
        _isAtTargetRotation = true;
    }
}
