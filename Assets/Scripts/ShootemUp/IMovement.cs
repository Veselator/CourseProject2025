using UnityEngine;

[System.Serializable]
public struct Box
{
    // startPoint ¬—≈√ƒј будет левее и ниже endPoint
    public Vector2 startPoint;
    public Vector2 endPoint;

    public Box(Vector2 start, Vector2 end)
    {
        startPoint = start;
        endPoint = end;
    }
}

public interface IMovement
{
    Vector2 Velocity { get; set; }
    float Speed { get; set; }
    float MaxSpeed { get; set; }
    bool IsAbleToMove { get; set; }
    bool IsClamped { get; set; }

    virtual void Init(Vector2 velocity, float speed, float maxSpeed)
    {
        Speed = speed;
        Velocity = velocity;
        MaxSpeed = maxSpeed;
    }

    virtual void Init(Vector2 velocity)
    {
        Velocity = velocity;
    }

    virtual void SetIsClamped(bool isClamped)
    {
        IsClamped = isClamped;
    }

    virtual void ChangeSpeed(float speed)
    {
        Speed = speed;
    }
    virtual void SetMaxSpeed(float maxSpeed)
    {
        MaxSpeed = maxSpeed;
    }

    virtual void ChangeVelocity(Vector2 velocity)
    {
        Velocity = velocity.normalized;
    }

    virtual void Stop()
    {
        Velocity = Vector2.zero;
    }

    abstract void SetClampBorders(Vector2 min, Vector2 max);
}
