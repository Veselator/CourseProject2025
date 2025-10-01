using System.Collections;
using UnityEngine;

public class SurvivalCameraStartAnimation : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private float startSizeMultiplier = 2f;
    [SerializeField] private float startAnimationDuration = 1f;
    [SerializeField] private AnimationCurve easingCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private void Start()
    {
        cam = GetComponent<Camera>();
        float targetSize = cam.orthographicSize;
        cam.orthographicSize *= startSizeMultiplier;
        StartCoroutine(ZoomCameraLerp(targetSize, startAnimationDuration));
    }

    public IEnumerator ZoomCameraLerp(float targetSize, float duration)
    {
        float startSize = cam.orthographicSize;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            // ѕримен€ем кривую дл€ более плавного движени€
            float curvedT = easingCurve.Evaluate(t);

            cam.orthographicSize = Mathf.Lerp(startSize, targetSize, curvedT);

            yield return null;
        }

        cam.orthographicSize = targetSize;
    }
}