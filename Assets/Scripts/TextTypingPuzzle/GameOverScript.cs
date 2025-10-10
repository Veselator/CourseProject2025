using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverScript : MonoBehaviour
{
   [SerializeField] private GameObject gameoverscreen;
   [SerializeField] private TMP_Text gameOverText;
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
        gameOverText.text = $"Game Over!!!\nCorrect words:{CorrectWordsCount.Instance.correctWords}";


        gameoverscreen.SetActive(true);

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
