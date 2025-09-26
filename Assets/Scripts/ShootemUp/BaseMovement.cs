using UnityEngine;

public abstract class BaseMovement : MonoBehaviour, IMovement
{
    public Vector2 Velocity { get; set; }
    [Min(0.1f)]
    [SerializeField] private float speed;
    public float Speed
    {
        get => speed; set => speed = value;
    }
    private Box clampBox { get; set; }
    public float MaxSpeed { get; set; } // При текущей реализации не используется
    public bool IsAbleToMove { get; set; } = true;
    protected bool isClamped = false;
    public bool IsClamped {  get { return isClamped; } set { isClamped = value; } }

    protected virtual void FixedUpdate()
    {
        if(IsAbleToMove) HandleMovement();
    }

    public void SetClampBorders(Vector2 min, Vector2 max)
    {
        clampBox = new Box(min, max);
    }

    protected Vector2 ClampPosition(Vector2 position2Clamp)
    {
        // Если структура не инициализирована
        if (clampBox.startPoint == Vector2.zero && clampBox.endPoint == Vector2.zero) return position2Clamp;

        //Debug.Log($"Currently clamping {position2Clamp}");
        Vector2 newClampedPosition = new(
            Mathf.Clamp(position2Clamp.x, clampBox.startPoint.x, clampBox.endPoint.x),
            Mathf.Clamp(position2Clamp.y, clampBox.startPoint.y, clampBox.endPoint.y)
        );

        return newClampedPosition;
    }

    protected abstract void HandleMovement();
}
