using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScaleIfHighlightedUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float _scaleFactor = 1.1f;
    [SerializeField] private float _scaleDuration = 0.2f;

    private Vector3 _originalScale;
    private Coroutine _scaleCoroutine;

    private void Start()
    {
        _originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ScaleUp();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ScaleDown();
    }

    private void ScaleUp()
    {
        if (_scaleCoroutine != null)
            StopCoroutine(_scaleCoroutine);
        _scaleCoroutine = StartCoroutine(ScaleTo(_originalScale * _scaleFactor));
    }

    private void ScaleDown()
    {
        if (_scaleCoroutine != null)
            StopCoroutine(_scaleCoroutine);
        _scaleCoroutine = StartCoroutine(ScaleTo(_originalScale));
    }

    private IEnumerator ScaleTo(Vector3 targetScale)
    {
        Vector3 startScale = transform.localScale;
        float time = 0f;

        while (time < _scaleDuration)
        {
            transform.localScale = Vector3.Lerp(startScale, targetScale, time / _scaleDuration);
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
    }
}
