using UnityEngine;

public class StraightMiddleMovingPattern : BaseMovingPattern
{
    private int pointsToMiddle;
    private int pointsDown;

    public StraightMiddleMovingPattern(int pointsToMid = 5, int pointsDownward = 10)
    {
        pointsToMiddle = pointsToMid;
        pointsDown = pointsDownward;
    }

    public override void Init()
    {
        Vector2 middlePoint = new Vector2(0f, 0f);
        pathPoints = new Vector2[pointsToMiddle + pointsDown];

        // Точки до центра
        for (int i = 0; i < pointsToMiddle; i++)
        {
            float t = (float)i / (pointsToMiddle - 1);
            pathPoints[i] = Vector2.Lerp(StartPosition, middlePoint, t);
        }

        // Точки вниз от центра
        for (int i = 0; i < pointsDown; i++)
        {
            float t = (float)i / (pointsDown - 1);
            pathPoints[pointsToMiddle + i] = Vector2.Lerp(middlePoint, EndPosition, t);
        }

        Reset();
    }
}