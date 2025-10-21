using UnityEngine;

public class ObjectFlipperManager : MonoBehaviour
{
    // Реализаует поворот на основе RigidbodyPlatformerMovement
    [SerializeField] private Transform _object2Flip;
    private RigidbodyPlatformerMovement _movement;
    private bool _isFlipped = false;

    private void Start()
    {
        _movement = GetComponent<RigidbodyPlatformerMovement>();

        _movement.OnAnyMove += HandleFlip;
    }

    private void OnDestroy()
    {
        _movement.OnAnyMove -= HandleFlip;
    }

    private void Flip()
    {
        _object2Flip.localScale = new Vector3(-_object2Flip.localScale.x, 
            _object2Flip.localScale.y, 
            _object2Flip.localScale.z);
    }

    private void HandleFlip()
    {
        if (_movement.CurrentVelocityX > 0f && _isFlipped)
        {
            _isFlipped = false;
            Flip();
        }
        else if (_movement.CurrentVelocityX < 0f && !_isFlipped)
        {
            _isFlipped = true;
            Flip();
        }
    }
}
