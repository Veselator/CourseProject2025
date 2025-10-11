using System.Collections;
using UnityEngine;

public class ItemVisual : MonoBehaviour
{
    [Header("Colors")]
    [SerializeField] private Color highlightedColor = Color.white;
    [SerializeField] private Color standardColor = Color.gray;

    [Header("Animation")]
    [SerializeField] private float transitionDuration = 0.2f; // Длительность перехода

    private SpriteRenderer _spriteRenderer;
    private bool isHighlighted = false;
    private Coroutine currentTransition; // Для отмены предыдущей корутины

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (_spriteRenderer == null)
        {
            Debug.LogError($"SpriteRenderer not found on {gameObject.name}!");
            enabled = false;
        }
    }

    private void Start()
    {
        _spriteRenderer.color = standardColor;
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
        Color startColor = _spriteRenderer.color;
        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / transitionDuration;

            // Используем Lerp для плавной интерполяции
            _spriteRenderer.color = Color.Lerp(startColor, targetColor, t);

            yield return null;
        }

        // Гарантируем точное конечное значение
        _spriteRenderer.color = targetColor;
        currentTransition = null;
    }
}