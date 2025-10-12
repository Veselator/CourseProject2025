using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemVisualUI : MonoBehaviour
{
    [Header("Colors")]
    [SerializeField] private Color highlightedColor = Color.white;
    [SerializeField] private Color standardColor = Color.gray;

    [Header("Animation")]
    [SerializeField] private float transitionDuration = 0.2f; // Длительность перехода

    private Image _image;
    private bool isHighlighted = false;
    private Coroutine currentTransition; // Для отмены предыдущей корутины

    private void Awake()
    {
        _image = GetComponent<Image>();

        if (_image == null)
        {
            Debug.LogError($"Image not found on {gameObject.name}!");
            enabled = false;
        }
    }

    private void Start()
    {
        _image.color = standardColor;
    }

    public void Highlight(bool newIsHighlighted)
    {
        // Если состояние не изменилось - ничего не делаем
        if (newIsHighlighted == isHighlighted) return;

        isHighlighted = newIsHighlighted;

        // Останавливаем предыдущую анимацию, если она идёт
        if (currentTransition != null)
        {
            StopCoroutine(currentTransition);
        }

        // Запускаем новую анимацию
        Color targetColor = isHighlighted ? highlightedColor : standardColor;
        currentTransition = StartCoroutine(TransitionColor(targetColor));
    }

    private IEnumerator TransitionColor(Color targetColor)
    {
        Color startColor = _image.color;
        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / transitionDuration;

            // Используем Lerp для плавной интерполяции
            _image.color = Color.Lerp(startColor, targetColor, t);

            yield return null;
        }

        // Гарантируем точное конечное значение
        _image.color = targetColor;
        currentTransition = null;
    }
}
