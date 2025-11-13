using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuWindowAppearingAnimation : MonoBehaviour
{
    [Header("Окна")]
    [SerializeField] private RectTransform[] _windows;

    [Header("Точки анимации")]
    [SerializeField] private Transform _topPoint;
    [SerializeField] private Transform _bottomPoint;

    [Header("Настройки анимации")]
    [SerializeField] private float _animationDuration = 0.5f;
    [SerializeField] private AnimationCurve _animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    private float _backgroundTransperencyStartValue;

    [SerializeField] private Image _backgroundImage;

    private Coroutine[] _activeCoroutines;

    private void Awake()
    {
        _activeCoroutines = new Coroutine[_windows.Length];

        foreach (RectTransform window in _windows)
        {
            if (window != null)
            {
                window.gameObject.SetActive(false);
            }
        }

        _backgroundImage.gameObject.SetActive(true);
        _backgroundImage.raycastTarget = false;
        _backgroundTransperencyStartValue = _backgroundImage.color.a;
        _backgroundImage.color = new Color(0, 0, 0, 0);
    }

    public void ShowWindow(int id)
    {
        if (!IsValidWindowId(id)) return;

        if (_activeCoroutines[id] != null)
        {
            StopCoroutine(_activeCoroutines[id]);
        }

        _backgroundImage.raycastTarget = true;
        _windows[id].gameObject.SetActive(true);
        _activeCoroutines[id] = StartCoroutine(AnimateWindow(_windows[id], _topPoint.position, _bottomPoint.position));
    }

    public void HideWindow(int id)
    {
        if (!IsValidWindowId(id)) return;

        if (_activeCoroutines[id] != null)
        {
            StopCoroutine(_activeCoroutines[id]);
        }

        _backgroundImage.raycastTarget = false;
        _activeCoroutines[id] = StartCoroutine(AnimateWindowAndHide(_windows[id], _windows[id].position, _topPoint.position, id));
    }

    private bool IsValidWindowId(int id)
    {
        if (id < 0 || id >= _windows.Length)
        {
            Debug.LogError($"Неверный ID окна: {id}. Доступно окон: {_windows.Length}");
            return false;
        }

        if (_windows[id] == null)
        {
            Debug.LogError($"Окно с ID {id} не назначено!");
            return false;
        }

        return true;
    }

    private IEnumerator AnimateWindow(RectTransform window, Vector3 startPos, Vector3 endPos)
    {
        float elapsedTime = 0f;
        Color backgroundColor = _backgroundImage.color;

        while (elapsedTime < _animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = _animationCurve.Evaluate(elapsedTime / _animationDuration);
            window.position = Vector3.Lerp(startPos, endPos, t);

            _backgroundImage.color = new Color(backgroundColor.r, backgroundColor.g, backgroundColor.b, t * _backgroundTransperencyStartValue);
            yield return null;
        }

        window.position = endPos;
        _activeCoroutines[System.Array.IndexOf(_windows, window)] = null;
    }

    private IEnumerator AnimateWindowAndHide(RectTransform window, Vector3 startPos, Vector3 endPos, int id)
    {
        float elapsedTime = 0f;
        Color backgroundColor = _backgroundImage.color;

        while (elapsedTime < _animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = _animationCurve.Evaluate(elapsedTime / _animationDuration);
            window.position = Vector3.Lerp(startPos, endPos, t);

            _backgroundImage.color = new Color(backgroundColor.r, backgroundColor.g, backgroundColor.b, (1 - t) * _backgroundTransperencyStartValue);
            yield return null;
        }

        window.position = endPos;
        window.gameObject.SetActive(false);
        _activeCoroutines[id] = null;
    }
}