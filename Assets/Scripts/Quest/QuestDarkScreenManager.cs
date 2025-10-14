using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDarkScreenManager : MonoBehaviour
{
    [SerializeField] private float fromAlphaValue = 0f;
    [SerializeField] private float toAlphaValue = 0.4f;
    [SerializeField] private float duration = 0.4f;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.color = new Color(0f, 0f, 0f, fromAlphaValue);
    }

    public void StartAnimation()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color color = _spriteRenderer.color;
        color.a = fromAlphaValue;
        _spriteRenderer.color = color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(fromAlphaValue, toAlphaValue, elapsedTime / duration);
            _spriteRenderer.color = color;
            yield return null;
        }

        color.a = toAlphaValue;
        _spriteRenderer.color = color;
    }
}
