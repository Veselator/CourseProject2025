using UnityEngine;

public class RigidbodyMovement : BaseMovement
{
    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        //ClampVelocity();
    }

    protected override void HandleMovement()
    {
        //_rb.AddForce(Velocity * Speed);
        Vector2 newPosition = _rb.position + Speed * Time.fixedDeltaTime * Velocity;

        if (isClamped) newPosition = ClampPosition(newPosition);

        _rb.MovePosition(newPosition);
    }
}