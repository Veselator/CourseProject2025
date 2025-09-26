using UnityEngine;

public class StraightMovingPattern : BaseMovingPattern
{
    private int pointsCount;

    public StraightMovingPattern(int points = 10)
    {
        pointsCount = points;
    }

    public override void Init()
    {
        pathPoints = new Vector2[pointsCount];

        for (int i = 0; i < pointsCount; i++)
        {
            float t = (float)i / (pointsCount - 1);
            pathPoints[i] = Vector2.Lerp(StartPosition, EndPosition, t);
        }

        Reset();
    }
}