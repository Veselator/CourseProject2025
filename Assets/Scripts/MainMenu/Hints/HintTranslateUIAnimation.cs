using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintTranslateUIAnimation : BaseHint
{
    [SerializeField] private RectTransform StartPoint, EndPoint;
    private RectTransform _rectTransform;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _rectTransform.position = StartPoint.position;
    }

    protected override IEnumerator Animation()
    {
        float _currentTime = 0f;

        while (_currentTime < _animationDuration)
        {
            _currentTime += Time.deltaTime;

            float t = _currentTime / _numOfRepeats;
            t = _animationCurve.Evaluate(t);
            _rectTransform.position = Vector2.Lerp(StartPoint.position, EndPoint.position, t);

            yield return null;
        }
    }
}
