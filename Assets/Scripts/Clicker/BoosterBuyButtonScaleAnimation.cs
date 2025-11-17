using UnityEngine;
using System.Collections;

public class BoosterBuyButtonScaleAnimation : MonoBehaviour
{
    [Header("Ссылки")]
    [SerializeField] private UIBooster _linkedBooster;
    [SerializeField] private Transform _targetTransform;

    [Header("Настройки анимации")]
    [SerializeField] private float _animationSpeed = 2f;
    [SerializeField] private float _scaleAmplitude = 0.1f;
    [SerializeField] private bool _useUnscaledTime = false;
    [SerializeField] private AnimationCurve _scaleCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Оси анимации")]
    [SerializeField] private bool _animateX = true;
    [SerializeField] private bool _animateY = true;
    [SerializeField] private bool _animateZ = false;

    private Vector3 _initialScale;
    private Coroutine _animationCoroutine;
    private bool _isAnimating = false;

    private void Awake()
    {
        if (_targetTransform == null)
        {
            _targetTransform = transform;
        }

        _initialScale = _targetTransform.localScale;
    }

    private void Start()
    {
        if (_linkedBooster != null)
        {
            _linkedBooster.OnBoosterAvailable += PlayScaleAnimation;
            _linkedBooster.OnBoosterNotAvailable += ResetScale;
        }
    }

    private void OnDestroy()
    {
        if (_linkedBooster != null)
        {
            _linkedBooster.OnBoosterAvailable -= PlayScaleAnimation;
            _linkedBooster.OnBoosterNotAvailable -= ResetScale;
        }
    }

    private void PlayScaleAnimation()
    {
        if (!_targetTransform.gameObject.activeInHierarchy) return;
        if (_animationCoroutine != null)
        {
            StopCoroutine(_animationCoroutine);
        }

        _isAnimating = true;
        _animationCoroutine = StartCoroutine(ScaleAnimationCoroutine());
    }

    private void ResetScale()
    {
        _isAnimating = false;

        if (_animationCoroutine != null)
        {
            StopCoroutine(_animationCoroutine);
            _animationCoroutine = null;
        }

        if (_targetTransform.gameObject.activeInHierarchy) _targetTransform.localScale = _initialScale;
    }

    private IEnumerator ScaleAnimationCoroutine()
    {
        float time = 0f;

        while (_isAnimating)
        {
            time += (_useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime) * _animationSpeed;

            float sinValue = Mathf.Sin(time);
            float curveValue = _scaleCurve.Evaluate((sinValue + 1f) / 2f);
            float scaleFactor = 1f + (curveValue * 2f - 1f) * _scaleAmplitude;

            Vector3 newScale = _initialScale;

            if (_animateX) newScale.x = _initialScale.x * scaleFactor;
            if (_animateY) newScale.y = _initialScale.y * scaleFactor;
            if (_animateZ) newScale.z = _initialScale.z * scaleFactor;

            _targetTransform.localScale = newScale;

            yield return null;
        }
    }

    public void SetAnimationSpeed(float speed)
    {
        _animationSpeed = speed;
    }

    public void SetScaleAmplitude(float amplitude)
    {
        _scaleAmplitude = amplitude;
    }

    public void StopAnimation()
    {
        ResetScale();
    }
}