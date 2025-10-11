using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class PlayerPointController : MonoBehaviour
{
    [SerializeField] private Transform[] points = new Transform[4];
    public float moveDuration = 0.44f;
    public AnimationCurve easeInCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private void Start()
    {
        GameObject startPoint = new GameObject("StartPoint");
        startPoint.transform.position = transform.position;
        startPoint.transform.parent = transform.parent;

        points[0] = startPoint.transform;
    }

    private void OnEnable()
    {
        GlobalFlags.onFlagChanged += OnGlobalFlagChanged;
    }

    private void OnDisable()
    {
        GlobalFlags.onFlagChanged -= OnGlobalFlagChanged;
    }

    private void OnGlobalFlagChanged(string flag, bool flagState)
    {
        if (flag == Flags.RunnerStage1Passed.ToString() && flagState)
        {
            MoveTo(points[1], -2f);
        }
        if (flag == Flags.RunnerStage2Passed.ToString() && flagState)
        {
            // Да, hard-code
            // Задаём смещение точек при анимации
            //MoveTo(points[0], -2f);
            StartCoroutine(StartSequenceOfMoving(new Transform[]{ points[2], points[0] },
                new Vector2[]{ new Vector2(-2f, 0f), new Vector2(0f, 0f) }, new float[] { moveDuration, moveDuration / 2f }, 2));
            //MoveTo(points[0], 2f, -2f);
        }
    }

    private void MoveTo(Transform outTransfrom, float yMidpointOffset = 0, float xMidPointOffset = 0, float endYOffset = 0, float endXOffset = 0)
    {
        if (outTransfrom == null) return;
        StartCoroutine(MoveToPointCoroutine(outTransfrom, moveDuration, true, yMidpointOffset, xMidPointOffset, endYOffset, endXOffset));
    }

    private IEnumerator StartSequenceOfMoving(Transform[] points, Vector2[] offsetPoints, float[] timeToEnd, int countOfElements)
    {
        // Устанавливаем глобальный флаг при старте последовательности
        //GlobalFlags.SetFlag(GlobalFlags.Flags.RUNNER_IS_ROTATING);

        int i = 0;
        Vector2 currentOffset;
        float currentDuration;
        //bool isEnding = false;

        foreach (Transform point in points)
        {
            if (point == null) continue;

            try
            {
                currentOffset = offsetPoints[i];
                currentDuration = timeToEnd[i];
                //isEnding = i == countOfElements - 1;
            }
            catch
            {
                Debug.LogError("PlayerPointController: Error in moving sequence.");
                break;
            }

            // Ожидаем конца корутины перед началом следующей
            yield return StartCoroutine(MoveToPointCoroutine(point, currentDuration, true, currentOffset.y, currentOffset.x, 0f, 0f));
            i++;
        }
    }

    private IEnumerator MoveToPointCoroutine(Transform outPoint, float movingDuration, bool isEnding, float yOffset, float xOffset, float endYOffset, float endXOffset)
    {
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;
        outPoint.transform.position += new Vector3(endXOffset, endYOffset, 0);

        // Создаем контрольную точку для дуги
        Vector3 midPoint = (startPosition + outPoint.position) * 0.5f;
        midPoint.y += yOffset; // Опускаем середину для создания дуги
        midPoint.x += xOffset; // Опускаем середину для создания дуги

        float elapsedTime = 0f;
        while (elapsedTime < movingDuration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / movingDuration;
            float curveValue = easeInCurve.Evaluate(normalizedTime);

            // Квадратичная кривая Безье
            Vector3 bezierPosition = CalculateQuadraticBezier(startPosition, midPoint, outPoint.position, curveValue);

            transform.position = bezierPosition;
            transform.rotation = Quaternion.Slerp(startRotation, outPoint.rotation, curveValue);

            yield return null;
        }

        transform.position = outPoint.position;
        transform.rotation = outPoint.rotation;

        if (isEnding && GlobalFlags.GetFlag(Flags.RunnerIsRotating))
            // GlobalFlags.SetFlag(GlobalFlags.Flags.RUNNER_IS_ROTATING);
            GlobalFlags.ClearFlag(Flags.RunnerIsRotating);
    }

    // Вспомогательный метод для расчета квадратичной кривой Безье
    private Vector3 CalculateQuadraticBezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        return uu * p0 + 2 * u * t * p1 + tt * p2;
    }
}
