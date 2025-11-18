using System.Collections;
using UnityEngine;

public class HintScaleAnimation : BaseHint
{
    [SerializeField] private float _fromScale = 0.5f, _toScale = 1.2f;
    private Vector3 _startScale;
    private RectTransform _rectTransform;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _startScale = _rectTransform.localScale;
    }

    protected override IEnumerator Animation()
    {
        float _currentTime = 0f;
        _rectTransform.localScale = _startScale * _fromScale;

        while (_currentTime < _animationDuration)
        {
            _currentTime += Time.deltaTime;
            float t = _currentTime / _animationDuration;
            t = _animationCurve.Evaluate(t);

            float currentScale = Mathf.Lerp(_fromScale, _toScale, t);
            _rectTransform.localScale = _startScale * currentScale;

            yield return null;
        }

        _rectTransform.localScale = _startScale * _toScale;
    }
}