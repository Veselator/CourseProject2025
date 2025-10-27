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
    public bool IsGrounded => _isGrounded;
    private bool _isGroundedLast = false; // Нужно для отслеживания состояния

    private RigidbodyPlatformerMovement _rigidbodyMovement;
    protected override bool IsHandleAdditionalThings { get; } = true;

    public event Action OnPlayerJumped;
    public event Action OnPlayerWalking;
    public event Action OnPlayerDoesntWalking;
    public event Action OnPlayerFalling;
    public event Action OnPlayerGrounded;
    public event Action OnPlayerDegrounded;

    protected override void Init()
    {
        _playerInput = PlayerInput.Instance;
        _movement = GetComponent<IMovement>();
        _rigidbodyMovement = _movement as RigidbodyPlatformerMovement;

        OnPlayerJumped += Jump;

        _movement.Init(Vector2.zero);
    }

    private void OnDestroy()
    {
        OnPlayerJumped -= Jump;
    }

    protected override void HandleInput()
    {
        base.HandleInput();
        HandleJump();
    }

    protected override void HandleMovingInput()
    {
        Vector2 currentMovementVector = MovementVector;

        if (currentMovementVector.x != 0) OnPlayerWalking?.Invoke();
        else OnPlayerDoesntWalking?.Invoke();

            _movement.ChangeVelocity(currentMovementVector);
    }

    protected override void HandleAdditionalThings()
    {
        // Родительский класс гарантирует, что вызов HandleAdditionalThings будет перед HandleInput
        HoldIsGrounded();

        CoyoteTimerHandler();
        AirTimerHandler();
        ExtraGravity();
    }

    private void HoldIsGrounded()
    {
        // Проверяем логику, связанную с приземлением и полётом
        _isGrounded = GetIsGrounded();
        if (_isGrounded) isSecondJumpAvailable = true;

        // Приземлились
        if(!_isGroundedLast && _isGrounded) OnPlayerGrounded?.Invoke();
        // От земли оттолкнулись
        else if (_isGroundedLast && !_isGrounded) OnPlayerDegrounded?.Invoke();
        // Летит
        else if (!_isGrounded) OnPlayerFalling?.Invoke();

        _isGroundedLast = _isGrounded;
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

    private bool GetIsGrounded()
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
            OnPlayerJumped?.Invoke();
        }
        else if (isSecondJumpAvailable)
        {
            _airTimer = 0f;
            OnPlayerJumped?.Invoke();
            isSecondJumpAvailable = false;
        }
    }

    private void Jump() => _rigidbodyMovement.HandleJump();
}
