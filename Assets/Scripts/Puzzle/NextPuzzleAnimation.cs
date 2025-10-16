using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextPuzzleAnimation : MonoBehaviour
{
    [SerializeField] private GameObject _target;
    [SerializeField] private float _maxScale = 1.2f; // ������������ �������
    [SerializeField] private float _totalDuration = 2f; // ����� ������������ ��������
    [SerializeField] private AnimationCurve _curve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private float _startScale = 0f; // ��������� �������
    private Image _image;
    private RectTransform _movingTransform; // Transform, ������� ����� ���������

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
        // ��������������� �������� ������ �������� � ������

        yield return StartCoroutine(ScaleUpCoroutine());

        _gm.GetToNextPuzzle();
        yield return null; // ���� ���� ����, ����� ���������, ��� ����� ���������
        _colorManager.AssignNewColorPalette();
        _puzzleSolvedAnimation.ResetPuzzleSolvedAnimation();

        yield return StartCoroutine(ScaleDownCoroutine());
    }

    // �������� ��� �������� ���������� _movingTransform �� 0 �� _maxScale
    private IEnumerator ScaleUpCoroutine()
    {
        float duration = _totalDuration / 2; // ������������ ��������
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

    // �������� ��� �������� ���������� _movingTransform �� _maxScale �� 0
    private IEnumerator ScaleDownCoroutine()
    {
        float duration = _totalDuration / 2; // ������������ ��������
        float elapsed = 0f;

        Color startColor = _image.color;
        Color endColor = _colorManager.CurrentPalette.secondColor;

        while (elapsed < duration)
        {
            // �����
            elapsed += Time.deltaTime;
            float curvedProgress = _curve.Evaluate(elapsed / duration);

            // ������� � ����
            float scale = Mathf.Lerp(_maxScale, _startScale, curvedProgress);
            Color currentColor = Color.Lerp(startColor, endColor, curvedProgress);

            // ����������
            _movingTransform.localScale = new Vector3(scale, scale, 1f);
            _image.color = currentColor;

            yield return null;
        }

        _movingTransform.localScale = new Vector3(_startScale, _startScale, 1f);
        _image.color = endColor;
    }
}
