using System.Collections;
using UnityEngine;

public class HintTranslateAnimation : BaseHint
{
    [SerializeField] private Transform StartPoint, EndPoint;

    private void Start()
    {
        transform.position = StartPoint.position;
    }

    protected override IEnumerator Animation()
    {
        float _currentTime = 0f;

        while (_currentTime < _animationDuration)
        {
            _currentTime += Time.deltaTime;

            float t = _currentTime / _animationDuration;
            t = _animationCurve.Evaluate(t);
            transform.position = Vector2.Lerp(StartPoint.position, EndPoint.position, t);

            yield return null;
        }
    }
}
