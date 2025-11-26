using UnityEngine;
using UnityEngine.Events;

public class CursorMovement : MonoBehaviour
{
    [System.Serializable]
    public class CursorMovingEvent : UnityEvent<Vector2> { }

    public CursorMovingEvent OnCursorMoving = new CursorMovingEvent();

    [SerializeField] private bool hideCursor = true;

    private Vector2 lastMousePosition;

    void Start()
    {
        if (hideCursor)
        {
            Cursor.visible = false;
        }

        lastMousePosition = Input.mousePosition;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        Vector2 currentMousePosition = Input.mousePosition;

        transform.position = currentMousePosition;

        if (currentMousePosition != lastMousePosition)
        {
            OnCursorMoving?.Invoke(currentMousePosition);
            lastMousePosition = currentMousePosition;
        }
    }
}
