using UnityEngine;

public class RandomMovingPattern : BaseMovingPattern
{
    private int randomPointsCount;
    private float randomRadius;

    public RandomMovingPattern(int pointsCount = 8, float radius = 2f)
    {
        randomPointsCount = pointsCount;
        randomRadius = radius;
    }

    public override void Init()
    {
        pathPoints = new Vector2[randomPointsCount];
        pathPoints[0] = StartPosition;

        // √енерируем случайные промежуточные точки
        for (int i = 1; i < randomPointsCount - 1; i++)
        {
            float t = (float)i / (randomPointsCount - 1);
            Vector2 basePoint = Vector2.Lerp(StartPosition, EndPosition, t);
            Vector2 randomOffset = Random.insideUnitCircle * randomRadius;
            pathPoints[i] = basePoint + randomOffset;
        }

        pathPoints[randomPointsCount - 1] = EndPosition;
        Reset();
    }
}