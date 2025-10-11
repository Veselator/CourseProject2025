using System.Collections;
using UnityEngine;

public class ItemVisual : MonoBehaviour
{
    [Header("Colors")]
    [SerializeField] private Color highlightedColor = Color.white;
    [SerializeField] private Color standardColor = Color.gray;

    [Header("Animation")]
    [SerializeField] private float transitionDuration = 0.2f; // ������������ ��������

    private SpriteRenderer _spriteRenderer;
    private bool isHighlighted = false;
    private Coroutine currentTransition; // ��� ������ ���������� ��������

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
        // ���� ��������� �� ���������� - ������ �� ������
        if (newIsHighlighted == isHighlighted) return;

        isHighlighted = newIsHighlighted;

        // ������������� ���������� ��������, ���� ��� ���
        if (currentTransition != null)
        {
            StopCoroutine(currentTransition);
        }

        // ��������� ����� ��������
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

            // ���������� Lerp ��� ������� ������������
            _spriteRenderer.color = Color.Lerp(startColor, targetColor, t);

            yield return null;
        }

        // ����������� ������ �������� ��������
        _spriteRenderer.color = targetColor;
        currentTransition = null;
    }
}