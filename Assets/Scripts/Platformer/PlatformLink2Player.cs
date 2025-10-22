using UnityEngine;

public class PlatformLink2Player : MonoBehaviour
{
    private Vector2 _lastPosition;
    private Vector2 _platformVelocity;

    private void Start()
    {
        _lastPosition = transform.position;
    }

    private void FixedUpdate()
    {
        Vector2 currentPosition = transform.position;
        _platformVelocity = (currentPosition - _lastPosition) / Time.fixedDeltaTime;
        _lastPosition = currentPosition;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        RigidbodyPlatformerMovement movement = collision.gameObject.GetComponent<RigidbodyPlatformerMovement>();

        // Если у объекта нет компонента движения ИЛИ если его позиция ниже допустимой - пропускаем, нам это не надо
        if (!movement || collision.transform.position.y < transform.position.y + 0.5f) return;
        movement.SetPlatformVelocity(_platformVelocity);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        RigidbodyPlatformerMovement movement = collision.gameObject.GetComponent<RigidbodyPlatformerMovement>();

        if (!movement) return;
        movement.SetPlatformVelocity(Vector2.zero);
    }
}