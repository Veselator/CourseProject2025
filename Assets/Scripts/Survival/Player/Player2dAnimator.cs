using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2dANimator : MonoBehaviour
{
    public static Player2dANimator Instance { get; private set; }
    public bool IsFlipped { get; private set; } = false;
    public bool IsMoving { get; private set; } = false;
    public bool IsRunning { get; private set; }

    private Rigidbody2D _rigidBody;
    private Animator _animator;
    private Player_Movement _playerMovement;
    [SerializeField] private float flipThreshold = 0.1f; // Минимальная скорость для переворота
    [SerializeField] private float movingThreshold = 0.1f; // Минимальная скорость движения

    private void Awake()
    {
        if (Instance == null) Instance = this;
        _playerMovement = Player_Movement.Instance;

        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        CheckFlip();
        CheckIsMoving();
    }

    private void CheckIsMoving()
    {
        bool isAnyMove = (Mathf.Abs(_rigidBody.velocity.x) > movingThreshold) || (Mathf.Abs(_rigidBody.velocity.y) > movingThreshold);

        //if (isAnyMove) Debug.Log($"Velocity: {_rigidBody.velocity}");

        if (isAnyMove && !IsMoving)
        {
            IsMoving = true;
            ChangeCharacterAnimation();
        }
        else if (!isAnyMove && IsMoving)
        {
            IsMoving = false;
            ChangeCharacterAnimation();
        }

        if(isAnyMove) CheckIsRunning();
    }

    private void CheckIsRunning()
    {
        bool isCurrentlyRunning = _playerMovement.CurrentSpeed > _playerMovement.speed;
        if (isCurrentlyRunning && !IsRunning)
        {
            IsRunning = true;
            _animator.speed = 2f;
        }
        else if (!isCurrentlyRunning && IsRunning)
        {
            IsRunning = false;
            _animator.speed = 1f;
        }
    }

    private void ChangeCharacterAnimation()
    {
        _animator.SetBool("IsMoving", IsMoving);
    }

    private void CheckFlip()
    {
        // Проверяем, движется ли персонаж (игнорируем минимальные движения)
        if (Mathf.Abs(_rigidBody.velocity.x) < flipThreshold)
            return;

        // Если движется влево и не перевёрнут - переворачиваем
        if (_rigidBody.velocity.x < 0 && IsFlipped)
        {
            Flip();
        }
        // Если движется вправо и перевёрнут - переворачиваем обратно
        else if (_rigidBody.velocity.x > 0 && !IsFlipped)
        {
            Flip();
        }
    }

    private void Flip()
    {
        IsFlipped = !IsFlipped;

        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}