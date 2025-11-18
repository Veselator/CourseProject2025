using UnityEngine;
using System.Collections;
using System;

public class UIBouncyMover : MonoBehaviour
{
    [Header("Точки движения")]
    [SerializeField] private RectTransform _startPoint;
    [SerializeField] private RectTransform _endPoint;

    [Header("Настройки движения")]
    [SerializeField] private float _startDelay = 0f;
    [SerializeField] private float _moveDuration = 1f;
    [SerializeField] private AnimationCurve _moveEasing = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Настройки прыжков")]
    [SerializeField] private bool _enableBounce = true;
    [SerializeField] private int _bounceCount = 3;
    [SerializeField] private float _initialBounceHeight = 50f;
    [SerializeField, Range(0f, 1f)] private float _bounceDamping = 0.6f;
    [SerializeField] private float _bounceSpeed = 2f;

    [Header("Дополнительные эффекты")]
    [SerializeField] private bool _rotateOnBounce = false;
    [SerializeField] private float _rotationAmount = 360f;

    private RectTransform _rectTransform;

    public event Action OnMoveCompleted;
    public event Action OnBounceHit;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        _rectTransform.position = _startPoint.position;
        StartCoroutine(StartWithDelay());
    }

    private IEnumerator StartWithDelay()
    {
        if (_startDelay > 0f)
        {
            yield return new WaitForSeconds(_startDelay);
        }

        yield return StartCoroutine(MoveAnimation());
    }

    private IEnumerator MoveAnimation()
    {
        if (_startPoint == null || _endPoint == null)
        {
            Debug.LogError("StartPoint или EndPoint не назначены!");
            yield break;
        }

        _rectTransform.anchoredPosition = _startPoint.anchoredPosition;

        Vector2 startPos = _startPoint.anchoredPosition;
        Vector2 endPos = _endPoint.anchoredPosition;
        float elapsed = 0f;

        while (elapsed < _moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / _moveDuration;
            float easedT = _moveEasing.Evaluate(t);

            Vector2 currentPos = Vector2.Lerp(startPos, endPos, easedT);
            _rectTransform.anchoredPosition = currentPos;

            yield return null;
        }

        _rectTransform.anchoredPosition = endPos;

        if (_enableBounce)
        {
            yield return StartCoroutine(BounceAnimation());
        }

        OnMoveCompleted?.Invoke();
    }

    private IEnumerator BounceAnimation()
    {
        Vector2 basePosition = _rectTransform.anchoredPosition;
        float currentBounceHeight = _initialBounceHeight;
        Quaternion startRotation = _rectTransform.rotation;

        for (int i = 0; i < _bounceCount; i++)
        {
            float bounceTime = 0f;
            float bounceCycleDuration = 1f / _bounceSpeed;

            while (bounceTime < bounceCycleDuration)
            {
                bounceTime += Time.deltaTime;
                float t = bounceTime / bounceCycleDuration;

                float height = Mathf.Sin(t * Mathf.PI) * currentBounceHeight;
                _rectTransform.anchoredPosition = basePosition + Vector2.up * height;

                if (_rotateOnBounce)
                {
                    float rotation = Mathf.Sin(t * Mathf.PI) * _rotationAmount / _bounceCount;
                    _rectTransform.rotation = startRotation * Quaternion.Euler(0, 0, rotation);
                }

                yield return null;
            }

            OnBounceHit?.Invoke();
            currentBounceHeight *= _bounceDamping;
        }

        _rectTransform.anchoredPosition = basePosition;
        if (_rotateOnBounce)
        {
            _rectTransform.rotation = startRotation;
        }
    }
}