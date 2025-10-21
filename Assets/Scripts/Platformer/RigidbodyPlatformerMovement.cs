using System;
using UnityEngine;

public class RigidbodyPlatformerMovement : RigidbodyMovement
{
    // Отвечает непосредственно за физику движения

    private float _targetVelocityX = 0f;
    private float _currentVelocityX = 0f;
    public float CurrentVelocityX => _currentVelocityX;
    private bool _isAtTargetSpeed = true;

    private const float _movementThreshold = 0.01f;
    [Header("Настройка движения")]
    [SerializeField] private float _lerpFactorForMovement = 0.02f;
    [SerializeField] private float _lerpFactorForRotate = 0.05f;
    [SerializeField] private float _lerpFactorIfNoButtonPressed = 0.05f;
    [Header("Настройка гравитации")]
    [SerializeField] private float _extraGravity = 700f;
    private float _currentLerpFactor = 0f;
    // Мусорная переменная чисто для Mathf.SmoothDamp
    private float JustRefVariableForSmoothDumpBecauseItRequiresSomeVariableButSimultaneouslyReturnsValueWhatTheShitIAmWritingIsntIt;

    public bool NoButtonPressed => _targetVelocityX == 0f;
    public event Action OnAnyMove;
    protected override void FixedUpdate()
    {
        if(!_isAtTargetSpeed) HandleSmoothMovement();
        base.FixedUpdate();
    }

    private bool IsDifferentSings(float a, float b)
    {
        return (a * b) < 0f;
    }

    private void HandleSmoothMovement()
    {
        // Lerp - НЕ КРУТО для физики движения
        //_currentVelocityX = Mathf.Lerp(_currentVelocityX, _targetVelocityX, _currentLerpFactor);
        // Mathf.SmoothDamp - круто для физики движения
        _currentVelocityX = Mathf.SmoothDamp(_currentVelocityX, _targetVelocityX, ref JustRefVariableForSmoothDumpBecauseItRequiresSomeVariableButSimultaneouslyReturnsValueWhatTheShitIAmWritingIsntIt, _currentLerpFactor);

        //Debug.Log($"CurrentVelocityX = {_currentVelocityX} _currentLerpFactor = {_currentLerpFactor}");
        if(Mathf.Abs(_currentVelocityX - _targetVelocityX) < _movementThreshold)
        {
            _currentVelocityX = _targetVelocityX;
            _isAtTargetSpeed = true;
        }
    }

    public override void ChangeVelocity(Vector2 newVecloity)
    {
        _targetVelocityX = newVecloity.x;
        _isAtTargetSpeed = false;

        if (NoButtonPressed)
        {
            _currentLerpFactor = _lerpFactorIfNoButtonPressed;
        }
        else if (IsDifferentSings(_targetVelocityX, _currentVelocityX))
        {
            _currentLerpFactor = _lerpFactorForRotate;
        }
        else
        {
            _currentLerpFactor = _lerpFactorForMovement;
        }
    }

    public void HandleGravity()
    {
        _rigidbody.AddForce(new Vector2(0f, -_extraGravity * Time.deltaTime));
    }

    public override void HandleJump()
    {
        ResetGravity();
        _rigidbody.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);
    }

    protected override void HandleMovement()
    {
        //Vector2 newPosition = _rb.position + Speed * Time.fixedDeltaTime * Velocity;

        //_rb.MovePosition(newPosition);
        if (_currentVelocityX != 0) OnAnyMove?.Invoke();
        _rigidbody.velocity = new Vector2(Speed * _currentVelocityX * Time.fixedDeltaTime, _rigidbody.velocity.y);
    }

    public void ResetGravity()
    {
        _rigidbody.velocity = Vector2.zero;
    }

    public void DoImpulse(Vector2 direction, float impulseStrength = 1f)
    {
        //Debug.Log($"Impulse direction {direction} impulseStrength {impulseStrength}");
        ResetGravity();
        _rigidbody.AddForce(direction * impulseStrength, ForceMode2D.Impulse);
    }
}
