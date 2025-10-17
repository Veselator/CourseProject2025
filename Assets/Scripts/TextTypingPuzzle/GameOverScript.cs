using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverScript : MonoBehaviour
{
   [SerializeField] private GameObject gameoverscreen;
    [SerializeField] private Transform parrants;

    private void ClearAllWords()
    {
        foreach (Transform child in parrants)
        {
            Destroy(child.gameObject);
        }

    }
    private void GameOverScreen()
    {
        ClearAllWords();

        gameoverscreen.GetComponent<CanvasGroup>().alpha = 0f;
        gameoverscreen.SetActive(true);
        StartCoroutine(FadeInCanvasGroup(gameoverscreen.GetComponent<CanvasGroup>()));
    }

    IEnumerator FadeInCanvasGroup(CanvasGroup canvasGroup)
    {
        float duration = 1f; // Duration of the fade-in effect
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsedTime / duration);
            yield return null;
        }
        canvasGroup.alpha = 1f; // Ensure it's fully visible at the end
    }

    private void OnEnable()
    {
        EventManagerPuzzle.gameOverAction += GameOverScreen;

    }
    private void OnDisable()
    {
        EventManagerPuzzle.gameOverAction -= GameOverScreen;
    }
}
