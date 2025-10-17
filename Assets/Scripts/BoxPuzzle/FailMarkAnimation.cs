using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailMarkAnimation : MonoBehaviour
{
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private float scaleMultiplier = 1.2f;
    private Vector3 startScale = Vector3.zero;
    private Vector3 targetScale;

    private void Start()
    {
        targetScale = transform.localScale * scaleMultiplier;
        transform.localScale = startScale;
        StartCoroutine(PlayFailAnimation());
    }

    // �������� ������� ease-in ���������� ������ �� �������� ��������� � ����� ��������� �������
    private IEnumerator PlayFailAnimation()
    {
        float halfDuration = animationDuration / 2f;
        float timer = 0f;
        // ����������

        while (timer < halfDuration)
        {
            float t = timer / halfDuration;
            transform.localScale = Vector3.Lerp(startScale, targetScale, t * t);
            timer += Time.deltaTime;
            yield return null;
        }

        // ��������� ��� �������� ������������� �������
        transform.localScale = targetScale;
        timer = 0f;
        // ����������
        while (timer < halfDuration)
        {
            float t = timer / halfDuration;
            transform.localScale = Vector3.Lerp(targetScale, startScale, t * t);
            timer += Time.deltaTime;
            yield return null;
        }
        // ��������� ��� ��������� � ��������� �������
        transform.localScale = startScale;
        Destroy(gameObject);
    }
}
