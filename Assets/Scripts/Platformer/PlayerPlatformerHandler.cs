using System;
using UnityEngine;

public class PlayerPlatformerHandler : PlayerMovementHandler
{
    // Отвечает за управление игроком - соединяет ввод и физику
    // НАМ ВСЁ РАВНО как игрок вводит - геймпад, клавиатура или бананы. За это отвечает система Input Action
    // МЫ БЕЗ ПОНЯТИЯ как будет двигаться персонаж. За это отвечает RigidbodyPlatformerMovement
    // МЫ соединяем эти компоненты

    // Логика для корректной работы прыжка
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Transform _checkGroundTransform;
    public Transform BottomCheckTransform => _checkGroundTransform;
    [SerializeField] private Vector2 _groundCheckRectangle;
    public Vector2 BottomCheckRectangle => _groundCheckRectangle;

    // Custom gravity
    [SerializeField] private float _gravityDelay = 0.2f;

    private float _airTimer = 0f;
    private float _coyoteTimer = 0f;
    [SerializeField] private float _maxCoyoteTime = 0.4f;
    private bool isSecondJumpAvailable = true;
    private bool _isGrounded = false;

    private RigidbodyPlatformerMovement _rigidbodyMovement;
    protected override bool IsHandleAdditionalThings { get; } = true;

    public event Action OnPlayerJump;
    protected override void Init()
    {
        _playerInput = PlayerInput.Instance;
        _movement = GetComponent<IMovement>();
        _rigidbodyMovement = _movement as RigidbodyPlatformerMovement;

        OnPlayerJump += Jump;

        _movement.Init(Vector2.zero);
    }

    private void OnDestroy()
    {
        OnPlayerJump -= Jump;
    }

    protected override void HandleInput()
    {
        base.HandleInput();
        HandleJump();
    }

    protected override void HandleAdditionalThings()
    {
        // Родительский класс гарантирует, что вызов HandleAdditionalThings будет перед HandleInput
        _isGrounded = IsGrounded();
        if (_isGrounded) isSecondJumpAvailable = true;

        CoyoteTimerHandler();
        AirTimerHandler();
        ExtraGravity();
    }

    // Кастомная гравитация. Реализация от Rigidbody2D sucks

    private void AirTimerHandler()
    {
        if (_isGrounded)
        {
            _airTimer = 0f;
        }
        else
        {
            _airTimer += Time.deltaTime;
        }
    }

    private void ExtraGravity()
    {
        if (_airTimer > _gravityDelay)
        {
            _rigidbodyMovement.HandleGravity();
        }
    }

    private void CoyoteTimerHandler()
    {
        if (_isGrounded)
        {
            _coyoteTimer = _maxCoyoteTime;
        }
        else
        {
            _coyoteTimer -= Time.deltaTime;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapBox(_checkGroundTransform.position, _groundCheckRectangle, 0f, _groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_checkGroundTransform.position, _groundCheckRectangle);
    }

    private void HandleJump()
    {
        if (!_playerInput.IsHitButtonPressed()) return;

        if (_isGrounded || _coyoteTimer > 0f)
        {
            OnPlayerJump?.Invoke();
        }
        else if (isSecondJumpAvailable)
        {
            _airTimer = 0f;
            OnPlayerJump?.Invoke();
            isSecondJumpAvailable = false;
        }
    }

    private void Jump() => _rigidbodyMovement.HandleJump();
}
