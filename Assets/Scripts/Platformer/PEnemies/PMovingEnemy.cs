using System;
using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PMovingEnemy : BasePEnemy
{
    // Враг, который может перемещаться между точками + прыгать при необходимости

    [SerializeField] Transform[] points;
    [SerializeField] float time2Wait = 2f;
    [SerializeField] private bool _isLooped = true;
    private int _currentPoint = 0;
    private Vector2 _currentDirection;
    private Vector2 _targetPosition;
    private bool _isAtTarget = false;
    [SerializeField] private float _distanceThreshold = 0.4f;

    [SerializeField] private LayerMask _obstacleLayer;
    [SerializeField] private Vector2 _obstacleCheckSize = new Vector2(0.5f, 0.5f);
    [SerializeField] private float _obstacleCheckDistance = 1f;
    [SerializeField] private Transform _obstacleCheckTransform;
    // Кешируем значение результата поиска препятствий
    private Collider2D[] _obstacleResults = new Collider2D[10];

    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Transform _checkGroundTransform;
    [SerializeField] private Vector2 _groundCheckRectangle = new Vector2(0.8f, 0.2f);

    [SerializeField] private float _jumpCooldown = 0.5f;
    private float _lastJumpTime = -999f;

    [SerializeField] private float _gravityDelay = 0.2f;
    private float _airTimer = 0f;
    private float _coyoteTimer = 0f;
    [SerializeField] private float _maxCoyoteTime = 0.2f;

    private bool _isGrounded = false;
    private RigidbodyPlatformerMovement _rigidbodyMovement;

    public event Action OnPointReached;

    protected override void Start()
    {
        base.Start();
        _rigidbodyMovement = _movement as RigidbodyPlatformerMovement;
        SetTarget(_currentPoint);
        OnPointReached += UpdateTarget;
    }

    private void OnDestroy()
    {
        OnPointReached -= UpdateTarget;
    }

    private void Update()
    {
        if (_isAtTarget) return;

        HandleGroundDetection();
        CoyoteTimerHandler();
        AirTimerHandler();

        CheckIsAtTarget();
        HandleMovement();
        HandleObstacleDetection();
    }

    private void SetTarget(int id)
    {
        Debug.Log($"The target position changed! It`s {id}");
        _targetPosition = points[id].position;
        _isAtTarget = false;
        _currentDirection = (_targetPosition - (Vector2)transform.position).normalized;
    }

    // Корутина для реализации ожидания
    private IEnumerator SetTargetCoroutine(float time)
    {
        _currentDirection = Vector2.zero;
        _movement.ChangeVelocity(_currentDirection);
        yield return new WaitForSeconds(time);
        SetTarget(_currentPoint);
    }

    private void UpdateTarget()
    {
        _currentPoint++;
        if (_currentPoint >= points.Length)
        {
            if (_isLooped)
            {
                _currentPoint = 0;
            }
            else
            {
                return;
            }
        }

        if (time2Wait == 0f) SetTarget(_currentPoint);
        else StartCoroutine(SetTargetCoroutine(time2Wait));
    }

    private void CheckIsAtTarget()
    {
        if (Vector2.Distance(transform.position, _targetPosition) < _distanceThreshold)
        {
            _isAtTarget = true;
            OnPointReached?.Invoke();
        }
    }

    private void HandleMovement()
    {
        // Интерфейс движения - такой же, как у игрока
        _movement.ChangeVelocity(_currentDirection);
    }

    private void HandleObstacleDetection()
    {
        Vector2 horizontalDirection = new Vector2(_currentDirection.x, 0).normalized;

        if (horizontalDirection.magnitude < 0.1f) return;

        Vector2 checkPosition = _obstacleCheckTransform != null
            ? _obstacleCheckTransform.position
            : (Vector2)transform.position;

        checkPosition += horizontalDirection * _obstacleCheckDistance;

        int count = Physics2D.OverlapBoxNonAlloc(
            checkPosition,
            _obstacleCheckSize,
            0f,
            _obstacleResults,
            _obstacleLayer
        );

        for (int i = 0; i < count; i++)
        {
            if (_obstacleResults[i].gameObject == gameObject) continue;

            TryJump();
            break;
        }
    }

    private void TryJump()
    {
        bool canJump = (_isGrounded || _coyoteTimer > 0f) &&
                       (Time.time - _lastJumpTime > _jumpCooldown);

        if (canJump)
        {
            HandleJump();
            _lastJumpTime = Time.time;
        }
    }

    private void HandleJump()
    {
        if (_rigidbodyMovement != null)
        {
            _rigidbodyMovement.HandleJump();
            _airTimer = 0f;
        }
        else
        {
            _movement.HandleJump();
        }
    }

    private void HandleGroundDetection()
    {
        _isGrounded = IsGrounded();
    }

    private bool IsGrounded()
    {
        if (_checkGroundTransform == null) return false;

        return Physics2D.OverlapBox(
            _checkGroundTransform.position,
            _groundCheckRectangle,
            0f,
            _groundLayer
        );
    }

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

    private void OnDrawGizmos()
    {
        if (_checkGroundTransform != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(_checkGroundTransform.position, _groundCheckRectangle);
        }

        if (_obstacleCheckTransform != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(_obstacleCheckTransform.position, _obstacleCheckSize);
        }
    }
}