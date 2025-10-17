using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AccuracyUITextManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text _trackedText;
    [SerializeField] private TypingGameplay _typingGameplay;
    [SerializeField] private Gradient _colorGradient;

    [Header("Animation Settings")]
    [SerializeField] private AnimationType _animationType = AnimationType.PopScale;
    [SerializeField] private float _animationDuration = 0.3f;
    [SerializeField] private AnimationCurve _animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Pop Animation")]
    [SerializeField] private float _popScale = 1.3f;

    [Header("Shake Animation")]
    [SerializeField] private float _shakeStrength = 10f;
    [SerializeField] private int _shakeVibrato = 10;

    [Header("Number Counter Animation")]
    [SerializeField] private bool _useCounterAnimation = true;
    [SerializeField] private float _counterDuration = 0.5f;

    [Header("Pulse Animation")]
    [SerializeField] private float _pulseScale = 1.1f;
    [SerializeField] private float _pulseDuration = 0.2f;

    private float _currentDisplayedAccuracy = 0f;
    private float _targetAccuracy = 0f;
    private Coroutine _counterCoroutine;
    private Vector3 _originalScale;
    private Vector3 _originalPosition;

    public enum AnimationType
    {
        None,
        PopScale,
        Shake,
        Bounce,
        Pulse,
        SlideIn,
        Typewriter,
        Wave
    }

    private void Awake()
    {
        _typingGameplay.OnCharacterTyped += UpdateAccuracyText;
        _originalScale = _trackedText.transform.localScale;
        _originalPosition = _trackedText.transform.localPosition;
    }

    private void Start()
    {
        UpdateAccuracyText(0, '\0');
    }

    private void OnDestroy()
    {
        _typingGameplay.OnCharacterTyped -= UpdateAccuracyText;

        if (_counterCoroutine != null)
        {
            StopCoroutine(_counterCoroutine);
        }
    }

    private void UpdateAccuracyText(int _, char typedChar)
    {
        float accuracy = _typingGameplay.CurrentAccuracy;
        _targetAccuracy = accuracy;

        // Анимация изменения числа
        if (_useCounterAnimation)
        {
            if (_counterCoroutine != null)
            {
                StopCoroutine(_counterCoroutine);
            }
            _counterCoroutine = StartCoroutine(AnimateCounter(accuracy));
        }
        else
        {
            SetAccuracyDisplay(accuracy);
        }

        // Дополнительная анимация в зависимости от типа
        PlayAnimation(accuracy);

        // Специальные эффекты для определенных значений
        //CheckSpecialEffects(accuracy);
    }

    private void SetAccuracyDisplay(float accuracy)
    {
        _currentDisplayedAccuracy = accuracy;
        _trackedText.text = $"{accuracy * 100f:F0}% / 100%";

        Color textColor = _colorGradient.Evaluate(accuracy);
        _trackedText.color = textColor;
    }

    private IEnumerator AnimateCounter(float targetValue)
    {
        float startValue = _currentDisplayedAccuracy;
        float elapsedTime = 0f;

        while (elapsedTime < _counterDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = _animationCurve.Evaluate(elapsedTime / _counterDuration);

            float currentValue = Mathf.Lerp(startValue, targetValue, t);
            _trackedText.text = $"{currentValue * 100f:F0}% / 100%";

            // Плавное изменение цвета
            Color textColor = _colorGradient.Evaluate(currentValue);
            _trackedText.color = textColor;

            yield return null;
        }

        SetAccuracyDisplay(targetValue);
    }

    private void PlayAnimation(float accuracy)
    {
        switch (_animationType)
        {
            case AnimationType.PopScale:
                StartCoroutine(PopScaleAnimation());
                break;
            case AnimationType.Shake:
                StartCoroutine(ShakeAnimation(accuracy));
                break;
            case AnimationType.Bounce:
                StartCoroutine(BounceAnimation());
                break;
            case AnimationType.Pulse:
                StartCoroutine(PulseAnimation());
                break;
            case AnimationType.SlideIn:
                StartCoroutine(SlideInAnimation());
                break;
            case AnimationType.Typewriter:
                StartCoroutine(TypewriterAnimation());
                break;
            case AnimationType.Wave:
                StartCoroutine(WaveAnimation());
                break;
        }
    }

    private IEnumerator PopScaleAnimation()
    {
        float elapsedTime = 0f;

        while (elapsedTime < _animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = _animationCurve.Evaluate(elapsedTime / _animationDuration);

            if (elapsedTime < _animationDuration / 2)
            {
                float scaleUp = Mathf.Lerp(1f, _popScale, t * 2);
                _trackedText.transform.localScale = _originalScale * scaleUp;
            }
            else
            {
                float scaleDown = Mathf.Lerp(_popScale, 1f, (t - 0.5f) * 2);
                _trackedText.transform.localScale = _originalScale * scaleDown;
            }

            yield return null;
        }

        _trackedText.transform.localScale = _originalScale;
    }

    private IEnumerator ShakeAnimation(float accuracy)
    {
        // Трясем сильнее при низкой точности
        float intensity = _shakeStrength * (1f - accuracy);

        float elapsedTime = 0f;
        Vector3 startPos = _originalPosition;

        while (elapsedTime < _animationDuration)
        {
            elapsedTime += Time.deltaTime;

            float x = Random.Range(-intensity, intensity);
            float y = Random.Range(-intensity, intensity);

            _trackedText.transform.localPosition = startPos + new Vector3(x, y, 0);

            yield return null;
        }

        _trackedText.transform.localPosition = _originalPosition;
    }

    private IEnumerator BounceAnimation()
    {
        float elapsedTime = 0f;
        float bounceHeight = 20f;

        while (elapsedTime < _animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _animationDuration;

            // Используем синусоиду для создания эффекта подпрыгивания
            float bounce = Mathf.Sin(t * Mathf.PI) * bounceHeight;

            _trackedText.transform.localPosition = _originalPosition + Vector3.up * bounce;

            yield return null;
        }

        _trackedText.transform.localPosition = _originalPosition;
    }

    private IEnumerator PulseAnimation()
    {
        float elapsedTime = 0f;

        while (elapsedTime < _pulseDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _pulseDuration;

            float scale = 1f + Mathf.Sin(t * Mathf.PI) * (_pulseScale - 1f);
            _trackedText.transform.localScale = _originalScale * scale;

            yield return null;
        }

        _trackedText.transform.localScale = _originalScale;
    }

    private IEnumerator SlideInAnimation()
    {
        float elapsedTime = 0f;
        Vector3 startPos = _originalPosition + Vector3.right * 100f;

        _trackedText.transform.localPosition = startPos;

        while (elapsedTime < _animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = _animationCurve.Evaluate(elapsedTime / _animationDuration);

            _trackedText.transform.localPosition = Vector3.Lerp(startPos, _originalPosition, t);

            yield return null;
        }

        _trackedText.transform.localPosition = _originalPosition;
    }

    private IEnumerator TypewriterAnimation()
    {
        string targetText = _trackedText.text;
        string currentText = "";

        for (int i = 0; i <= targetText.Length; i++)
        {
            currentText = targetText.Substring(0, i);
            _trackedText.text = currentText;

            yield return new WaitForSeconds(_animationDuration / targetText.Length);
        }
    }

    private IEnumerator WaveAnimation()
    {
        // Анимация волны для каждого символа
        _trackedText.ForceMeshUpdate();
        TMP_TextInfo textInfo = _trackedText.textInfo;

        float elapsedTime = 0f;

        while (elapsedTime < _animationDuration)
        {
            elapsedTime += Time.deltaTime;

            for (int i = 0; i < textInfo.characterCount; i++)
            {
                TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

                if (!charInfo.isVisible) continue;

                int materialIndex = charInfo.materialReferenceIndex;
                int vertexIndex = charInfo.vertexIndex;

                Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;

                float wave = Mathf.Sin((elapsedTime * 5f) + (i * 0.5f)) * 5f;

                vertices[vertexIndex + 0].y += wave;
                vertices[vertexIndex + 1].y += wave;
                vertices[vertexIndex + 2].y += wave;
                vertices[vertexIndex + 3].y += wave;
            }

            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                _trackedText.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
            }

            yield return null;
        }

        _trackedText.ForceMeshUpdate();
    }

    private void CheckSpecialEffects(float accuracy)
    {
        // Специальные эффекты для milestone значений
        if (Mathf.Approximately(accuracy, 1f))
        {
            StartCoroutine(PerfectScoreAnimation());
        }
        else if (Mathf.Approximately(accuracy, 0.5f))
        {
            StartCoroutine(HalfwayAnimation());
        }
        else if (accuracy < 0.3f && _currentDisplayedAccuracy >= 0.3f)
        {
            StartCoroutine(DangerAnimation());
        }
    }

    private IEnumerator PerfectScoreAnimation()
    {
        // Анимация для 100%
        for (int i = 0; i < 3; i++)
        {
            _trackedText.transform.localScale = _originalScale * 1.5f;
            yield return new WaitForSeconds(0.1f);
            _trackedText.transform.localScale = _originalScale;
            yield return new WaitForSeconds(0.1f);
        }

        // Добавляем искры или частицы если есть ParticleSystem
        ParticleSystem particles = GetComponentInChildren<ParticleSystem>();
        if (particles != null)
        {
            particles.Play();
        }
    }

    private IEnumerator HalfwayAnimation()
    {
        // Быстрая пульсация при 50%
        float originalFontSize = _trackedText.fontSize;

        _trackedText.fontSize = originalFontSize * 1.2f;
        yield return new WaitForSeconds(0.2f);
        _trackedText.fontSize = originalFontSize;
    }

    private IEnumerator DangerAnimation()
    {
        // Мигание при низкой точности
        Color originalColor = _trackedText.color;

        for (int i = 0; i < 3; i++)
        {
            _trackedText.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            _trackedText.color = originalColor;
            yield return new WaitForSeconds(0.1f);
        }
    }
}