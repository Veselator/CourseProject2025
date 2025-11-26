using UnityEngine;
using System.Collections;

public class CursorClickAnimation : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private float scaleMultiplier = 1.3f;
    [SerializeField] private float animationDuration = 0.2f;
    [SerializeField] private AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Vector3 originalScale;
    private Coroutine currentAnimation;
    private bool _isBlocked = false;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (!_isBlocked && Input.GetMouseButtonDown(0))
        {
            PlayClickAnimation();
        }
    }

    public void StopAndReset()
    {
        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
        }

        transform.localScale = originalScale;
        _isBlocked = true;
    }

    public void Resume()
    {
        _isBlocked = false;
    }

    public void PlayClickAnimation()
    {
        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
        }

        currentAnimation = StartCoroutine(ClickAnimationCoroutine());
    }

    private IEnumerator ClickAnimationCoroutine()
    {
        float elapsedTime = 0f;
        Vector3 targetScale = originalScale * scaleMultiplier;

        while (elapsedTime < animationDuration / 2)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / (animationDuration / 2);
            float curveValue = scaleCurve.Evaluate(progress);

            transform.localScale = Vector3.Lerp(originalScale, targetScale, curveValue);
            yield return null;
        }

        elapsedTime = 0f;

        while (elapsedTime < animationDuration / 2)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / (animationDuration / 2);
            float curveValue = scaleCurve.Evaluate(progress);

            transform.localScale = Vector3.Lerp(targetScale, originalScale, curveValue);
            yield return null;
        }

        transform.localScale = originalScale;
        currentAnimation = null;
    }
}