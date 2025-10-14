using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class QuestVignetteAnimator : MonoBehaviour
{
    [Header("Volume Settings")]
    [SerializeField] private Volume volume;

    [Header("Animation Settings")]
    [SerializeField] private float animationDuration = 1f;
    [SerializeField] private AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private bool unscaledTime = false;

    private Vignette vignette;
    private Coroutine currentAnimation;
    private float currentIntensity;

    private void Awake()
    {
        InitializeVignette();
    }

    private void InitializeVignette()
    {
        if (volume == null)
        {
            volume = GetComponent<Volume>();
        }

        if (volume == null)
        {
            Debug.LogError("QuestVignetteAnimator: Volume component not found!");
            return;
        }

        if (!volume.profile.TryGet(out vignette))
        {
            Debug.LogError("QuestVignetteAnimator: Vignette effect not found in Volume profile!");
            return;
        }

        currentIntensity = vignette.intensity.value;
    }

    public void AnimateValue(float newValue)
    {
        if (vignette == null)
        {
            InitializeVignette();
            if (vignette == null) return;
        }

        newValue = Mathf.Clamp01(newValue);

        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
        }

        currentAnimation = StartCoroutine(AnimateIntensity(currentIntensity, newValue));
    }

    public void AnimateValue(float newValue, float duration)
    {
        float originalDuration = animationDuration;
        animationDuration = duration;
        AnimateValue(newValue);
        animationDuration = originalDuration;
    }

    private IEnumerator AnimateIntensity(float startValue, float endValue)
    {
        float elapsed = 0f;
        currentIntensity = startValue;

        while (elapsed < animationDuration)
        {
            elapsed += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / animationDuration);
            float curvedT = animationCurve.Evaluate(t);

            currentIntensity = Mathf.Lerp(startValue, endValue, curvedT);
            vignette.intensity.value = currentIntensity;

            yield return null;
        }

        currentIntensity = endValue;
        vignette.intensity.value = endValue;
        currentAnimation = null;
    }

    public float GetCurrentIntensity()
    {
        return currentIntensity;
    }

    public void SetValueImmediate(float value)
    {
        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
            currentAnimation = null;
        }

        if (vignette != null)
        {
            currentIntensity = Mathf.Clamp01(value);
            vignette.intensity.value = currentIntensity;
        }
    }

    public void StopAnimation()
    {
        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
            currentAnimation = null;
        }
    }
}