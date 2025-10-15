using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextPuzzleButton : MonoBehaviour
{
    private Gm _gm;
    [SerializeField] private GameObject _nextPuzzleButton;

    private void Start()
    {
        _gm = Gm.Instance;
        _gm.OnPuzzleCompleted += ShowNextPuzzleButton;
        _nextPuzzleButton.SetActive(false);
    }

    private void OnDestroy()
    {
        _gm.OnPuzzleCompleted -= ShowNextPuzzleButton;
    }

    private void ShowNextPuzzleButton()
    {
        StartCoroutine(ShowButtonWithDelay());
    }

    // ������ ���������� ���������� ����� �������� ����� ������
    private IEnumerator ShowButtonWithDelay()
    {
        yield return new WaitForSeconds(1f); // �������� ����� ���������� ������
        _nextPuzzleButton.SetActive(true);
        CanvasGroup canvasGroup = _nextPuzzleButton.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = _nextPuzzleButton.AddComponent<CanvasGroup>();
        }

        canvasGroup.alpha = 0f;
        float duration = 1f; // ������������ ��������
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsed / duration);
            yield return null;
        }
        canvasGroup.alpha = 1f; // ���������, ��� ����� ����� 1 � �����
    }
}
