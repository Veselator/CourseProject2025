using UnityEngine;
using System.Collections;

public class AppearingMovingAnimation : MonoBehaviour
{
    [Header("Точки анимации")]
    [SerializeField] private Vector3 _startOffset;
    private Vector3 _startPosition;
    private Vector3 _endPosition;

    [Header("Настройки анимации")]
    [SerializeField] private float _duration = 0.5f;
    [SerializeField] private float _delay = 0f;
    [SerializeField] private AnimationCurve _animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Coroutine _animationCoroutine;

    private void Start()
    {
        _endPosition = transform.position;
        _startPosition = transform.position + _startOffset;
        transform.position = _startPosition;

        PlayAnimation();
    }

    public void PlayAnimation()
    {
        if (_animationCoroutine != null)
        {
            StopCoroutine(_animationCoroutine);
        }

        _animationCoroutine = StartCoroutine(AnimateMovement());
    }

    public void ResetToStart()
    {
        if (_startPosition != null)
        {
            transform.position = _startPosition;
        }
    }

    public void ResetToEnd()
    {
        if (_endPosition != null)
        {
            transform.position = _endPosition;
        }
    }

    private IEnumerator AnimateMovement()
    {
        if (_delay > 0f) yield return new WaitForSeconds(_delay);

        float elapsedTime = 0f;

        while (elapsedTime < _duration)
        {
            elapsedTime += Time.deltaTime;
            float t = _animationCurve.Evaluate(elapsedTime / _duration);
            transform.position = Vector3.Lerp(_startPosition, _endPosition, t);
            yield return null;
        }

        transform.position = _endPosition;
        _animationCoroutine = null;
    }
}