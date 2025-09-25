using System.Text;
using UnityEngine;

public abstract class BaseMovingPattern : IMovingPattern
{
    public Vector2 StartPosition { get; set; }
    public Vector2 EndPosition { get; set; }
    public bool IsCompleted { get; protected set; }

    protected int currentPointIndex = 0;
    protected Vector2[] pathPoints;

    public abstract void Init();

    public virtual Vector2 GetNext()
    {
        if (IsCompleted || pathPoints == null || currentPointIndex >= pathPoints.Length)
        {
            return EndPosition;
        }

        Vector2 nextPoint = pathPoints[currentPointIndex];
        currentPointIndex++;

        if (currentPointIndex >= pathPoints.Length)
        {
            IsCompleted = true;
        }

        return nextPoint;
    }

    public virtual void Reset()
    {
        currentPointIndex = 0;
        IsCompleted = false;
    }

    private string PathPoints2String()
    {
        StringBuilder sb = new StringBuilder();
        foreach (var point in pathPoints) {
            sb.AppendLine($" x: {point[0]} y: {point[1]}");
        }

        return sb.ToString();
    }

    public override string ToString()
    {
        return PathPoints2String();
    }
}