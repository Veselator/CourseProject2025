using UnityEngine;

public class CursorRotation : MonoBehaviour
{
    [SerializeField] private CursorMovement cursorMovement;

    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float minMovementThreshold = 0.1f;

    private Vector2 _lastPosition;
    private Quaternion targetRotation;

    private bool _isBlocked = false;

    void Start()
    {
        if (cursorMovement != null)
        {
            cursorMovement.OnCursorMoving.AddListener(OnCursorMoved);
            _lastPosition = Input.mousePosition;
        }
        else
        {
            Debug.LogWarning("CursorMovement не назначен!");
        }

        targetRotation = transform.rotation;
    }

    public void StopAndReset()
    {
        transform.rotation = Quaternion.identity;
        _isBlocked = true;
    }

    public void Resume()
    {
        _isBlocked = false;
    }

    void OnCursorMoved(Vector2 currentPosition)
    {
        Vector2 movementVector = currentPosition - _lastPosition;

        if (movementVector.magnitude > minMovementThreshold)
        {
            float angle = Mathf.Atan2(movementVector.y, movementVector.x) * Mathf.Rad2Deg - 90f;

            targetRotation = Quaternion.Euler(0, 0, angle);
        }

        _lastPosition = currentPosition;
        //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    void Update()
    {
        if(!_isBlocked) transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    void OnDestroy()
    {
        if (cursorMovement != null)
        {
            cursorMovement.OnCursorMoving.RemoveListener(OnCursorMoved);
        }
    }
}