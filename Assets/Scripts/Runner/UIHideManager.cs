using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHideManager : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private PlayerHealth _playerHealth;
    [SerializeField] private float fadeDuration = 5f;
    private const float delayBeforeEndOfAnimation = 2f;

    private UIAppearManager _UIAppearManager;

    private void Start()
    {
        _playerHealth = PlayerHealth.Instance;
        _playerHealth.OnPlayerDied += HideUI;

        _UIAppearManager = UIAppearManager.Instance;
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnDestroy()
    {
        _playerHealth.OnPlayerDied -= HideUI;
    }

    private void HideUI()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        float elapsedTime = 0f;
        float startAlpha = canvasGroup.alpha;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        yield return new WaitForSeconds(delayBeforeEndOfAnimation);
        _UIAppearManager.ShowUI();
    }
}
