using UnityEngine;

public class RigidbodyMovement : BaseMovement
{
    protected Rigidbody2D _rigidbody;

    public void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public override void HandleJump()
    {
        Debug.Log("Jump is not implemented to this class but you somehow called it. Congratulations!");
    }

    protected override void HandleMovement()
    {
        //_rb.AddForce(Velocity * Speed);
        Vector2 newPosition = _rigidbody.position + Speed * Time.fixedDeltaTime * Velocity;

        if (isClamped) newPosition = ClampPosition(newPosition);

        _rigidbody.MovePosition(newPosition);
    }
}