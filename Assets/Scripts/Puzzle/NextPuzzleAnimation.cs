using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextPuzzleAnimation : MonoBehaviour
{
    [SerializeField] private GameObject _target;
    [SerializeField] private float _maxScale = 1.2f; // Максимальный масштаб
    [SerializeField] private float _totalDuration = 2f; // Общая длительность анимации
    [SerializeField] private AnimationCurve _curve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private float _startScale = 0f; // Начальный масштаб
    private Image _image;
    private RectTransform _movingTransform; // Transform, который будет двигаться

    private CurrentMainColorManager _colorManager;
    private Gm _gm;
    private PuzzleSolvedAnimation _puzzleSolvedAnimation;

    private void Start()
    {
        _colorManager = CurrentMainColorManager.Instance;
        _gm = Gm.Instance;
        _puzzleSolvedAnimation = PuzzleSolvedAnimation.Instance;

        _image = _target.GetComponent<Image>();
        _movingTransform = _target.GetComponent<RectTransform>();

        if (_image == null || _movingTransform == null)
        {
            Debug.LogError("Image or RectTransform component missing on target GameObject.");
            return;
        }

        _movingTransform.localScale = new Vector3(_startScale, _startScale, 1f);
    }

    public void StartTransition()
    {
        _image.color = _colorManager.CurrentPalette.secondColor;
        StartCoroutine(TransitionCoroutine());
    }

    private IEnumerator TransitionCoroutine()
    {
        // Последовательно вызываем нужные анимации и методы

        yield return StartCoroutine(ScaleUpCoroutine());

        _gm.GetToNextPuzzle();
        yield return null; // Ждем один кадр, чтобы убедиться, что паззл обновился
        _colorManager.AssignNewColorPalette();
        _puzzleSolvedAnimation.ResetPuzzleSolvedAnimation();

        yield return StartCoroutine(ScaleDownCoroutine());
    }

    // Корутина для плавного увеличения _movingTransform от 0 до _maxScale
    private IEnumerator ScaleUpCoroutine()
    {
        float duration = _totalDuration / 2; // Длительность анимации
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float curvedProgress = _curve.Evaluate(elapsed / duration);

            float scale = Mathf.Lerp(_startScale, _maxScale, curvedProgress);
            _movingTransform.localScale = new Vector3(scale, scale, 1f);
            yield return null;
        }

        _movingTransform.localScale = new Vector3(_maxScale, _maxScale, 1f);
    }

    // Корутина для плавного уменьшения _movingTransform от _maxScale до 0
    private IEnumerator ScaleDownCoroutine()
    {
        float duration = _totalDuration / 2; // Длительность анимации
        float elapsed = 0f;

        Color startColor = _image.color;
        Color endColor = _colorManager.CurrentPalette.secondColor;

        while (elapsed < duration)
        {
            // Время
            elapsed += Time.deltaTime;
            float curvedProgress = _curve.Evaluate(elapsed / duration);

            // Масштаб и цвет
            float scale = Mathf.Lerp(_maxScale, _startScale, curvedProgress);
            Color currentColor = Color.Lerp(startColor, endColor, curvedProgress);

            // Применение
            _movingTransform.localScale = new Vector3(scale, scale, 1f);
            _image.color = currentColor;

            yield return null;
        }

        _movingTransform.localScale = new Vector3(_startScale, _startScale, 1f);
        _image.color = endColor;
    }
}
