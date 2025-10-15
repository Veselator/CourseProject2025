using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePieceVisual : MonoBehaviour
{
    [Header("Scale Settings")]
    [SerializeField] private float scaleSizeFactor = 1.5f;
    [SerializeField] private float scaleUpDuration = 1f;
    [SerializeField] private float scaleDownDuration = 1f;

    [Header("Optional Settings")]
    [SerializeField] private AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private bool useUnscaledTime = false;
    [SerializeField] private bool loopAnimation = false;
    [SerializeField] private float delayBetweenLoops = 0f;

    private Vector3 originalScale;
    private Coroutine currentScaleCoroutine;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    public void StartScaling()
    {
        if (currentScaleCoroutine != null)
        {
            StopCoroutine(currentScaleCoroutine);
        }
        currentScaleCoroutine = StartCoroutine(ScaleAnimation());
    }

    public void StopScaling()
    {
        if (currentScaleCoroutine != null)
        {
            StopCoroutine(currentScaleCoroutine);
            currentScaleCoroutine = null;
        }
        transform.localScale = originalScale;
    }

    private IEnumerator ScaleAnimation()
    {
        do
        {
            Vector3 targetScale = originalScale * scaleSizeFactor;

            // Увеличиваем размер
            yield return ScaleTo(targetScale, scaleUpDuration);

            // Уменьшаем обратно до оригинального размера
            yield return ScaleTo(originalScale, scaleDownDuration);

            if (loopAnimation && delayBetweenLoops > 0)
            {
                yield return useUnscaledTime ?
                    new WaitForSecondsRealtime(delayBetweenLoops) :
                    new WaitForSeconds(delayBetweenLoops);
            }

        } while (loopAnimation);
    }

    private IEnumerator ScaleTo(Vector3 targetScale, float duration)
    {
        Vector3 startScale = transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float deltaTime = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            elapsedTime += deltaTime;

            float normalizedTime = Mathf.Clamp01(elapsedTime / duration);
            float curveValue = scaleCurve.Evaluate(normalizedTime);

            transform.localScale = Vector3.Lerp(startScale, targetScale, curveValue);

            yield return null;
        }

        transform.localScale = targetScale;
    }

    // Публичные методы для управления анимацией
    public void SetScaleFactor(float newFactor)
    {
        scaleSizeFactor = newFactor;
    }

    public void SetDurations(float upDuration, float downDuration)
    {
        scaleUpDuration = upDuration;
        scaleDownDuration = downDuration;
    }

    public void ResetToOriginalScale()
    {
        transform.localScale = originalScale;
    }
}
