using System.Collections;
using TMPro;
using UnityEngine;

public class NextAvailabilityAnimation : MonoBehaviour
{
    // Текст в диалоге, который является сигналом для игрока что можно перейти к следующей реплике
    [SerializeField] private TMP_Text _linkedText;
    [SerializeField] private float _fadeInDuration = 0.4f;
    private bool _isPlayingCoroutine = false;

    private void Start()
    {
        // Устанавливаем прозрачность текста
        SetAlpha(0f);
    }

    private void SetAlpha(float a)
    {
        Color color = _linkedText.color;
        _linkedText.color = new Color(color.r, color.g, color.b, 0);
    }

    public void PlayAnimation()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        _isPlayingCoroutine = true;
        float duration = 0;
        float t = 0;

        Color color = _linkedText.color;
        while (duration < _fadeInDuration)
        {
            duration += Time.deltaTime;
            t = duration / _fadeInDuration;

            _linkedText.color = new Color(color.r, color.g, color.b, t);
            yield return null;
        }

        _linkedText.color = new Color(color.r, color.g, color.b, 1);
        _isPlayingCoroutine = false;
    }

    public void Hide()
    {
        if (_isPlayingCoroutine)
        {
            StopAllCoroutines();
            _isPlayingCoroutine = false;
        }
        SetAlpha(0f);
    }
}
