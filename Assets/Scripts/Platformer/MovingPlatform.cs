using System;
using System.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // Как вариант - сделать pathfinding по точкам отдельным компонентом
    // Т.к. используется и в MovingPlatform и PMovingEnemy

    [SerializeField] protected Transform[] points;
    [SerializeField] float time2Wait = 2f;
    [SerializeField] protected bool _isLooped = true;
    [SerializeField] protected float speed = 2f;

    private int _currentPoint = 1;
    private Vector2 _currentDirection;
    private Vector2 _targetPosition;
    private bool _isAtTarget = false;
    [SerializeField] private float _distanceThreshold = 0.4f;
    private Rigidbody2D _rigidbody;

    public event Action OnPointReached;
    public event Action OnMovementEnded; // Если не _isLooped

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        transform.position = points[0].position;

        SetTarget(_currentPoint);
        OnPointReached += UpdateTarget;
    }

    private void OnDestroy()
    {
        OnPointReached -= UpdateTarget;
    }

    private void FixedUpdate()
    {
        if (_isAtTarget) return;

        CheckIsAtTarget();
        HandleMovement();
    }

    private void SetTarget(int id)
    {
        Debug.Log($"The target position changed! It`s {id}");
        _targetPosition = points[id].position;
        _isAtTarget = false;
        _currentDirection = (_targetPosition - (Vector2)transform.position).normalized;
    }

    private IEnumerator SetTargetCoroutine(float time)
    {
        _currentDirection = Vector2.zero;
        HandleMovement();
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
                OnMovementEnded?.Invoke();
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
        _rigidbody.velocity = speed * Time.deltaTime * _currentDirection;
    }
}
