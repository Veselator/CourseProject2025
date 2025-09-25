using System.Collections;
using UnityEngine;

public class HealthIndicatorScript : MonoBehaviour
{
    private PlayerHealth _playerHealth;
    [SerializeField] private int currentHealthIndex;
    private bool _isAlive = true;
    private const float DEATH_ANIMATION_DURATION = 0.5f;

    // Параметры анимации
    private const float SCALE_UP_MULTIPLIER = 1.3f;
    private const float SCALE_DOWN_MULTIPLIER = 0.1f;
    private const float SCALE_UP_RATIO = 0.3f; // 30% времени на увеличение

    private Vector3 _originalScale;
    private RectTransform _rectTransform;

    private void Start()
    {
        _playerHealth = PlayerHealth.Instance;
        if (_playerHealth != null)
            _playerHealth.OnPlayerHit += HandleHealth;

        // Сохраняем оригинальный масштаб и получаем RectTransform
        _rectTransform = GetComponent<RectTransform>();
        if (_rectTransform != null)
            _originalScale = _rectTransform.localScale;
        else
        {
            Transform transform = this.transform;
            _originalScale = transform.localScale;
        }
    }

    private void OnDisable()
    {
        if (_playerHealth != null)
            _playerHealth.OnPlayerHit -= HandleHealth;
    }

    private void HandleHealth()
    {
        // Проверяем, затронуло ли изменение здоровья этот индикатор
        if (_playerHealth.Health > currentHealthIndex) return;

        // Если затронуло, но здоровье и так 0, ничего не делаем
        if (!_isAlive) return;

        _isAlive = false;
        StartCoroutine(DeathAnimation(DEATH_ANIMATION_DURATION));
    }

    private IEnumerator DeathAnimation(float animationDuration)
    {
        float scaleUpTime = animationDuration * SCALE_UP_RATIO;
        float scaleDownTime = animationDuration - scaleUpTime;

        // Фаза 1: Увеличение
        Vector3 targetScaleUp = _originalScale * SCALE_UP_MULTIPLIER;
        yield return StartCoroutine(ScaleTo(targetScaleUp, scaleUpTime));

        // Фаза 2: Резкое уменьшение
        Vector3 targetScaleDown = _originalScale * SCALE_DOWN_MULTIPLIER;
        yield return StartCoroutine(ScaleTo(targetScaleDown, scaleDownTime));

        // Отключаем объект
        gameObject.SetActive(false);
    }

    private IEnumerator ScaleTo(Vector3 targetScale, float duration)
    {
        Vector3 startScale = GetCurrentScale();
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float progress = elapsedTime / duration;
            // Используем разные кривые для разных фаз
            float easedProgress = targetScale.x > startScale.x ?
                EaseOutQuad(progress) : // Плавное увеличение
                EaseInCubic(progress);  // Резкое уменьшение

            Vector3 currentScale = Vector3.Lerp(startScale, targetScale, easedProgress);
            SetScale(currentScale);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        SetScale(targetScale);
    }

    private Vector3 GetCurrentScale()
    {
        return _rectTransform != null ? _rectTransform.localScale : transform.localScale;
    }

    private void SetScale(Vector3 scale)
    {
        if (_rectTransform != null)
            _rectTransform.localScale = scale;
        else
            transform.localScale = scale;
    }

    // Easing функции для более естественной анимации
    private float EaseOutQuad(float t)
    {
        return t * (2f - t);
    }

    private float EaseInCubic(float t)
    {
        return t * t * t;
    }
}