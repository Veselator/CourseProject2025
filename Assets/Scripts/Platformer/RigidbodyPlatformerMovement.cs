using System;
using UnityEngine;

public class RigidbodyPlatformerMovement : RigidbodyMovement
{
    // ќтвечает непосредственно за физику движени€

    private float _targetVelocityX = 0f;
    private float _currentVelocityX = 0f;
    private bool _isAtTargetSpeed = true;

    private const float _movementThreshold = 0.01f;
    [SerializeField] private float _lerpFactorForMovement = 0.02f;
    [SerializeField] private float _lerpFactorForRotate = 0.05f;
    [SerializeField] private float _extraGravity = 700f;
    private float _currentLerpFactor = 0f;

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
        _currentVelocityX = Mathf.Lerp(_currentVelocityX, _targetVelocityX, _currentLerpFactor);
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


        if (IsDifferentSings(_targetVelocityX, _currentVelocityX))
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
        _rigidbody.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);
    }

    protected override void HandleMovement()
    {
        //Vector2 newPosition = _rb.position + Speed * Time.fixedDeltaTime * Velocity;

        //_rb.MovePosition(newPosition);
        _rigidbody.velocity = new Vector2(Speed * _currentVelocityX * Time.fixedDeltaTime, _rigidbody.velocity.y);
    }
}
