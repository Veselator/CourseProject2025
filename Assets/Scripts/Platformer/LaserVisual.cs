using System.Collections;
using UnityEngine;

public class LaserVisual : MonoBehaviour
{
    [SerializeField] private Laser _linkedLaser;
    [SerializeField] private Transform[] _points;
    [SerializeField] private float _maxWidth = 0.5f;
    [SerializeField] private float _animationDuration = 0.3f;

    private LineRenderer _lineRenderer;
    private Coroutine _widthChangeCoroutine;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();

        if (_points == null || _points.Length == 0)
        {
            Debug.LogError("Массив точек пуст!");
            return;
        }

        _lineRenderer.positionCount = _points.Length;
        for (int i = 0; i < _points.Length; i++)
        {
            _lineRenderer.SetPosition(i, _points[i].position);
        }

        if (_linkedLaser != null)
        {
            _linkedLaser.OnVisibilityChanged += HandleLaserVisibilityChanged;
        }
    }

    private void OnDestroy()
    {
        if (_linkedLaser != null)
        {
            _linkedLaser.OnVisibilityChanged -= HandleLaserVisibilityChanged;
        }
    }

    private void HandleLaserVisibilityChanged(bool isVisible)
    {
        if (_widthChangeCoroutine != null)
        {
            StopCoroutine(_widthChangeCoroutine);
        }

        if (isVisible)
        {
            _lineRenderer.enabled = true;
            _widthChangeCoroutine = StartCoroutine(AnimateWidth(0f, _maxWidth));
        }
        else
        {
            _widthChangeCoroutine = StartCoroutine(AnimateWidth(_maxWidth, 0f));
        }
    }

    private IEnumerator AnimateWidth(float startWidth, float targetWidth)
    {
        // Возможная оптимизация: отключение компонента когда width = 0
        float elapsedTime = 0f;

        while (elapsedTime < _animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _animationDuration;

            float smoothT = 1f - Mathf.Pow(1f - t, 3f);

            float currentWidth = Mathf.Lerp(startWidth, targetWidth, smoothT);
            _lineRenderer.startWidth = currentWidth;
            _lineRenderer.endWidth = currentWidth;

            yield return null;
        }

        _lineRenderer.startWidth = targetWidth;
        _lineRenderer.endWidth = targetWidth;
    }
}