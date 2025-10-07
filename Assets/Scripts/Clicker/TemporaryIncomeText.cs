using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TemporaryIncomeText : MonoBehaviour
{
    [Header("Настройки анимации")]
    [SerializeField] private float YOffset = 2f;
    [SerializeField] private float animationDuration = 2f;
    [SerializeField] private float smoothTime = 0.3f; // Для SmoothDamp

    [Header("Настройки исчезновения")]
    [SerializeField] private float fadeStartDelay = 0.5f; // Когда начинать исчезать (от 0 до 1, где 1 = в конце анимации)
    [SerializeField] private AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0, 1, 1, 0); // Кривая исчезновения

    [Header("Дополнительные эффекты")]
    [SerializeField] private bool useScale = true;
    [SerializeField] private AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0, 0.5f, 1, 1.2f);

    private TextMeshProUGUI textComponent;
    private Color originalColor;
    private Vector3 originalScale;

    void Start()
    {
        // Получаем компонент текста
        textComponent = GetComponent<TextMeshProUGUI>();
        if (textComponent == null)
        {
            textComponent = GetComponentInChildren<TextMeshProUGUI>();
        }

        if (textComponent != null)
        {
            originalColor = textComponent.color;
        }

        originalScale = transform.localScale;

        StartCoroutine(FlyAnimation());
    }

    private IEnumerator FlyAnimation()
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = transform.position + transform.up * YOffset;
        Vector3 velocity = Vector3.zero;
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            float normalizedTime = elapsedTime / animationDuration;

            // Плавное движение вверх через SmoothDamp
            transform.position = Vector3.SmoothDamp(
                transform.position,
                endPosition,
                ref velocity,
                smoothTime
            );

            // Изменение прозрачности
            if (textComponent != null)
            {
                float alpha = 1f;

                if (normalizedTime >= fadeStartDelay)
                {
                    // Вычисляем прогресс исчезновения
                    float fadeProgress = (normalizedTime - fadeStartDelay) / (1f - fadeStartDelay);
                    alpha = fadeCurve.Evaluate(fadeProgress);
                }

                // Применяем альфа-канал
                Color newColor = originalColor;
                newColor.a = originalColor.a * alpha;
                textComponent.color = newColor;
            }

            // Опциональное изменение масштаба
            if (useScale)
            {
                float scaleMultiplier = scaleCurve.Evaluate(normalizedTime);
                transform.localScale = originalScale * scaleMultiplier;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Уничтожаем объект после завершения анимации
        Destroy(gameObject);
    }
}