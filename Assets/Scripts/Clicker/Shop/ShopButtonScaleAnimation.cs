using System.Collections;
using System.Linq;
using UnityEngine;

public class ShopButtonScaleAnimation : MonoBehaviour
{
    [SerializeField] private ClickerShopManager _clickerShopManager;
    private ClickerManager _clickerManager;

    [Header("Ссылки")]
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

    private void Start()
    {
        if(_targetTransform == null)
        {
            _targetTransform = gameObject.transform;
        }

        _initialScale = _targetTransform.localScale;

        _clickerManager = ClickerManager.Instance;
        _clickerManager.OnMoneyChanged += OnMoneyChanged;
    }

    private void OnDestroy()
    {
        _clickerManager.OnMoneyChanged -= OnMoneyChanged;
    }

    private void OnMoneyChanged(float newAmount)
    {
        if (_clickerShopManager.GetAffordableItems().Count() > 0)
        {
            PlayScaleAnimation();
        }
        else
        {
            ResetScale();
        }
    }

    private void PlayScaleAnimation()
    {
        if (!_targetTransform.gameObject.activeInHierarchy) return;
        if (_isAnimating) return;

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
