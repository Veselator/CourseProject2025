using System.Collections;
using UnityEngine;

public class BoxMaterialController : MonoBehaviour
{
    private Material _boxMaterial;
    private const float ANIMATION_IN_DURATION = 1.7f;
    private const float ANIMATION_OUT_DURATION = 1f;
    private const float DELAY_BEFORE_DISAPPEARING = 1.5f;

    private AnimationCurve _animationCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    private BlockSelectionManager _blockSelectionManager;

    private static readonly int TrackingProperty = Shader.PropertyToID("_Value");

    private void Start()
    {
        Init();

        StartCoroutine(AppearAnimation());
    }

    private void Init()
    {
        _boxMaterial = GetComponent<SpriteRenderer>().material;

        if(_boxMaterial != null)
        {
            Debug.Log(_boxMaterial.GetFloat(TrackingProperty));
        }

        _blockSelectionManager = BlockSelectionManager.Instance;

        _blockSelectionManager.OnSuccessAnimation += StartSuccessAnimation;
        _boxMaterial.SetFloat(TrackingProperty, 1f);
    }

    private void OnDestroy()
    {
        _blockSelectionManager.OnSuccessAnimation -= StartSuccessAnimation;
    }

    private void StartSuccessAnimation()
    {
        StartCoroutine(DisappearAnimation());
    }

    private IEnumerator AppearAnimation()
    {
        yield return StartCoroutine(AnimateProperty(1f, 0f, ANIMATION_IN_DURATION, _animationCurve));
        GlobalFlags.SetFlag(Flags.IsReadyToShowMiniCamera);
    }

    private IEnumerator DisappearAnimation()
    {
        GlobalFlags.ClearFlag(Flags.IsReadyToShowMiniCamera);
        yield return new WaitForSeconds(DELAY_BEFORE_DISAPPEARING);
        yield return StartCoroutine(AnimateProperty(0f, 1f, ANIMATION_OUT_DURATION, _animationCurve));
    }

    private IEnumerator AnimateProperty(float startValue, float endValue, float duration, AnimationCurve curve)
    {
        float elapsedTime = 0f;
        float value = startValue;
        float t;

        while (elapsedTime < duration)
        {
            t = elapsedTime / duration;
            value = Mathf.Lerp(startValue, endValue, curve.Evaluate(t));
            _boxMaterial.SetFloat(TrackingProperty, value);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _boxMaterial.SetFloat(TrackingProperty, endValue);
    }
}
