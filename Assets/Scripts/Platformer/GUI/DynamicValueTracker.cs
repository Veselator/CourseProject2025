using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DynamicValueTracker : MonoBehaviour
{
    [SerializeField] private Image _topImage;
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Image _negativeDeltaImage;
    [SerializeField] private Image _positiveDeltaImage;
    [SerializeField] private TMP_Text[] texts;
    [SerializeField] private float _negativeDeltaDelay = 0.3f;
    [SerializeField] private float _negativeDeltaAnimationTime = 1.5f;
    [SerializeField] private float _positiveDeltaAnimationTime = 2f;
    [SerializeField] private bool IsNeed2RenderText = true;

    private IReadableValue _trackedValue;
    private float _currentDisplayValue;
    private Coroutine _negativeDeltaCoroutine;
    private Coroutine _positiveDeltaCoroutine;

    private void OnDestroy()
    {
        if (_trackedValue != null)
        {
            _trackedValue.OnValueChanged -= HandleValueChanged;
        }
    }

    public void Initialize(IReadableValue value, float maxValue = 100f)
    {
        Debug.Log($"Initing value {value.Value}");

        if (_trackedValue != null)
        {
            _trackedValue.OnValueChanged -= HandleValueChanged;
        }

        _trackedValue = value;
        _trackedValue.OnValueChanged += HandleValueChanged;

        _currentDisplayValue = Mathf.Clamp01(_trackedValue.Value);
        UpdateAllImages(_currentDisplayValue);
        UpdateText(_currentDisplayValue);
    }

    private void HandleValueChanged(float newValue)
    {
        float normalizedValue = Mathf.Clamp01(newValue);
        float delta = normalizedValue - _currentDisplayValue;

        if (delta < 0)
        {
            HandleNegativeDelta(normalizedValue);
        }
        else if (delta > 0)
        {
            HandlePositiveDelta(normalizedValue);
        }

        _currentDisplayValue = normalizedValue;
        UpdateText(normalizedValue);
    }

    private void HandleNegativeDelta(float targetValue)
    {
        if (_negativeDeltaCoroutine != null)
        {
            StopCoroutine(_negativeDeltaCoroutine);
        }

        _positiveDeltaImage.fillAmount = 0f;
        _topImage.fillAmount = targetValue;
        _negativeDeltaCoroutine = StartCoroutine(NegativeDeltaCoroutine(targetValue));
    }

    private void HandlePositiveDelta(float targetValue)
    {
        if (_positiveDeltaCoroutine != null)
        {
            StopCoroutine(_positiveDeltaCoroutine);
        }

        _negativeDeltaImage.fillAmount = 0f;
        _positiveDeltaImage.fillAmount = targetValue;
        _positiveDeltaCoroutine = StartCoroutine(PositiveDeltaCoroutine(targetValue));
    }

    private IEnumerator NegativeDeltaCoroutine(float targetValue)
    {
        yield return new WaitForSeconds(_negativeDeltaDelay);

        float startValue = _negativeDeltaImage.fillAmount;
        float elapsed = 0f;

        while (elapsed < _negativeDeltaAnimationTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / _negativeDeltaAnimationTime;
            _negativeDeltaImage.fillAmount = Mathf.Lerp(startValue, targetValue, t);
            yield return null;
        }

        _negativeDeltaImage.fillAmount = targetValue;
    }

    private IEnumerator PositiveDeltaCoroutine(float targetValue)
    {
        float startValue = _topImage.fillAmount;
        float elapsed = 0f;

        while (elapsed < _positiveDeltaAnimationTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / _positiveDeltaAnimationTime;
            _topImage.fillAmount = Mathf.Lerp(startValue, targetValue, t);
            yield return null;
        }

        _topImage.fillAmount = targetValue;
        _positiveDeltaImage.fillAmount = targetValue;
    }

    private void UpdateAllImages(float value)
    {
        _topImage.fillAmount = value;
        _negativeDeltaImage.fillAmount = value;
        _positiveDeltaImage.fillAmount = value;
    }

    private void UpdateText(float normalizedValue)
    {
        if (!IsNeed2RenderText || texts == null || texts.Length == 0)
            return;

        string textToDisplay;

        float percentage = normalizedValue * 100f;
        textToDisplay = $"{percentage:F0}%";

        foreach (var text in texts)
        {
            if (text != null)
            {
                text.text = textToDisplay;
            }
        }
    }
}