using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAppearManager : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private float fadeDuration = 1.5f;

    public static UIAppearManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        canvasGroup.alpha = 0f;
    }

    public void ShowUI()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }
}
