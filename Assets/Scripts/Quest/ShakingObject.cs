using System.Collections;
using UnityEngine;

public class ShakingObject : MonoBehaviour
{
    // ������������� ������ ��� ������
    [Header("Shake Settings")]
    [SerializeField] private float shakeAmplitudeX = 10f;
    [SerializeField] private float shakeAmplitudeY = 10f;
    [SerializeField] private float shakeSpeed = 5f; // �������� ����������� � ��������� �����
    [SerializeField] private bool IsShakingOnAwake = false;

    private RectTransform rectTransform;
    private Transform regularTransform;
    private bool isRectTransform;

    private Vector3 originalPosition;
    private bool isShaking = false;
    private Coroutine shakeCoroutine;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        if (rectTransform != null)
        {
            isRectTransform = true;
            originalPosition = rectTransform.anchoredPosition;
        }
        else
        {
            isRectTransform = false;
            regularTransform = transform;
            originalPosition = regularTransform.localPosition;
        }

        if (IsShakingOnAwake) StartShaking();
    }

    public void StartShaking()
    {
        if (isShaking) return;

        isShaking = true;

        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }

        shakeCoroutine = StartCoroutine(ShakeCoroutine());
    }

    public void StopShaking()
    {
        isShaking = false;
    }

    private IEnumerator ShakeCoroutine()
    {
        while (isShaking)
        {
            // ���������� ��������� ����� ������ ������������ �������
            Vector3 randomOffset = new Vector3(
                Random.Range(-shakeAmplitudeX, shakeAmplitudeX),
                Random.Range(-shakeAmplitudeY, shakeAmplitudeY),
                0f
            );

            Vector3 targetPosition = originalPosition + randomOffset;

            // ������ ������������ � ������� �����
            yield return StartCoroutine(MoveToPosition(targetPosition));
        }

        // ������������ �� �������� ������� ��� ���������
        yield return StartCoroutine(MoveToPosition(originalPosition));
    }

    private IEnumerator MoveToPosition(Vector3 targetPos)
    {
        Vector3 startPos = GetCurrentPosition();
        float distance = Vector3.Distance(startPos, targetPos);
        float duration = distance / shakeSpeed;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            Vector3 newPos = Vector3.Lerp(startPos, targetPos, t);
            SetPosition(newPos);

            yield return null;

            // ���� ������ ������������ �� ����� ��������, ���������
            if (!isShaking && targetPos != originalPosition)
            {
                yield break;
            }
        }

        SetPosition(targetPos);
    }

    private Vector3 GetCurrentPosition()
    {
        if (isRectTransform)
        {
            return rectTransform.anchoredPosition;
        }
        else
        {
            return regularTransform.localPosition;
        }
    }

    private void SetPosition(Vector3 position)
    {
        if (isRectTransform)
        {
            rectTransform.anchoredPosition = position;
        }
        else
        {
            regularTransform.localPosition = position;
        }
    }

    // ��� ��������� �������� ������� (���� ������ ��� ���������)
    public void UpdateOriginalPosition()
    {
        originalPosition = GetCurrentPosition();
    }
}