using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAppearManager : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 1.5f;
    [SerializeField] private float delayBeforeStart = 0f;

    public static UIAppearManager Instance { get; private set; }
    public Action OnAppearAnimationEnded;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0f;
    }

    public void ShowUI()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        if(delayBeforeStart > 0f) yield return new WaitForSeconds(delayBeforeStart);
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            yield return null;
        }

        OnAppearAnimationEnded?.Invoke();
        canvasGroup.alpha = 1f;
    }
}
