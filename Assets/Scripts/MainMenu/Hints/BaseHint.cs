using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseHint : MonoBehaviour, IHint
{
    // Базовый класс подсказки
    // Да, решение не очень
    // Но подсказок мало + система далее не будет развиваться

    [SerializeField] private float _delayBefore = 0f;
    [SerializeField] protected float _animationDuration = 1.0f;
    [SerializeField] protected AnimationCurve _animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] protected int _numOfRepeats = 2;
    [SerializeField] protected float _delayBetweenRepeats = 0.2f;

    [Header("Настройки появления")]
    [SerializeField] private bool _enableStartAppearing = true;
    [SerializeField] private bool _enableFadeOut = true;
    [SerializeField] private float _fadeInOutDuration = 0.5f;

    private Image _image;
    private SpriteRenderer _spriteRenderer;
    private CanvasGroup _canvasGroup;
    private LinkedImages _linkedImages;
    private float _initialAlpha;

    private event Action<float> OnAlphaChanged;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _linkedImages = GetComponent<LinkedImages>();

        if (_image != null)
        {
            _initialAlpha = _image.color.a;
            OnAlphaChanged += SetAlphaOnImage;
        }
        else if (_spriteRenderer != null)
        {
            _initialAlpha = _spriteRenderer.color.a;
            OnAlphaChanged += SetAlphaOnSpriteRenderer;
        }
        else if (_canvasGroup != null)
        {
            _initialAlpha = _canvasGroup.alpha;
            OnAlphaChanged += SetAlphaOnCanvasGroup;
        }
        else if (_linkedImages != null)
        {
            _initialAlpha = _linkedImages.LinkedSprites[0].color.a;
            OnAlphaChanged += SetAlphaOnLinkedImages;
        }

        if (_enableStartAppearing) OnAlphaChanged?.Invoke(0f);
    }

    private void OnDestroy()
    {
        if (_image != null)
        {
            OnAlphaChanged -= SetAlphaOnImage;
        }
        else if (_spriteRenderer != null)
        {
            OnAlphaChanged -= SetAlphaOnSpriteRenderer;
        }
        else if (_canvasGroup != null)
        {
            OnAlphaChanged -= SetAlphaOnCanvasGroup;
        }
        else if (_linkedImages != null)
        {
            OnAlphaChanged -= SetAlphaOnLinkedImages;
        }
    }

    public void PlayAnimation()
    {
        StartCoroutine(RepeatedAnimation());
    }

    private IEnumerator RepeatedAnimation()
    {
        WaitForSeconds waitForSecondsDelay = new WaitForSeconds(_delayBetweenRepeats);
        if(_delayBefore > 0f) yield return new WaitForSeconds(_delayBefore);

        if (_enableStartAppearing) yield return StartCoroutine(FadeInAnimation());

        for (int i = 0; i < _numOfRepeats; i++)
        {
            yield return StartCoroutine(Animation());
            yield return waitForSecondsDelay;
        }

        if (_enableFadeOut) yield return StartCoroutine(FadeOutAnimation());

        gameObject.SetActive(false);
    }

    private IEnumerator FadeOutAnimation()
    {
        float elapsed = 0f;

        while (elapsed < _fadeInOutDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / _fadeInOutDuration;
            float alpha = Mathf.Lerp(_initialAlpha, 0f, t);

            OnAlphaChanged?.Invoke(alpha);

            yield return null;
        }

        OnAlphaChanged?.Invoke(0f);
    }

    private IEnumerator FadeInAnimation()
    {
        float elapsed = 0f;

        while (elapsed < _fadeInOutDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / _fadeInOutDuration;
            float alpha = Mathf.Lerp(0f, _initialAlpha, t);

            OnAlphaChanged?.Invoke(alpha);

            yield return null;
        }

        OnAlphaChanged?.Invoke(1f);
    }

    private void SetAlphaOnImage(float alpha)
    {
        Color color = _image.color;
        color.a = alpha;
        _image.color = color;
    }

    private void SetAlphaOnSpriteRenderer(float alpha)
    {
        Color color = _spriteRenderer.color;
        color.a = alpha;
        _spriteRenderer.color = color;
    }

    private void SetAlphaOnCanvasGroup(float alpha)
    {
        _canvasGroup.alpha = alpha;
    }

    private void SetAlphaOnLinkedImages(float alpha)
    {
        foreach (var image in _linkedImages.LinkedSprites)
        {
            Color color = image.color;
            color.a = alpha;
            image.color = color;
        }
    }

    protected abstract IEnumerator Animation();
}