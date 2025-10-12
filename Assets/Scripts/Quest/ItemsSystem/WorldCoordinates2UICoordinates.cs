using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCoordinates2UICoordinates : MonoBehaviour
{
    public static Vector3 Calculate(Vector3 worldCoordinates, Canvas canvas)
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldCoordinates);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screenPosition,
            null,//canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main,
            out Vector2 localPoint
        );

        return localPoint;
    }
}
