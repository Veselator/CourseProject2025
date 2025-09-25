using UnityEngine;

public class CurvedMovingPattern : BaseMovingPattern
{
    private Vector2 controlPoint1;
    private Vector2 controlPoint2;
    private int curveResolution;

    public CurvedMovingPattern(Vector2 control1, Vector2 control2, int resolution = 15)
    {
        controlPoint1 = control1;
        controlPoint2 = control2;
        curveResolution = resolution;
    }

    public override void Init()
    {
        pathPoints = new Vector2[curveResolution];

        for (int i = 0; i < curveResolution; i++)
        {
            float t = (float)i / (curveResolution - 1);
            pathPoints[i] = CalculateBezierPoint(t, StartPosition, controlPoint1, controlPoint2, EndPosition);
        }

        Reset();
    }

    private Vector2 CalculateBezierPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector2 point = uuu * p0;
        point += 3 * uu * t * p1;
        point += 3 * u * tt * p2;
        point += ttt * p3;

        return point;
    }
}