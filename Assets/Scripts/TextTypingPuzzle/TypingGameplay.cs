using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Windows;

public class TypingGameplay : MonoBehaviour
{

    private const float MIN_ACCURACY_THRESHOLD = 60f;
    private const float MIN_COMPLETION_THRESHOLD = 0.5f; // 50% слова

    [SerializeField] private CourotineSpawner spawner;
    [SerializeField] private CourotineWordMover mover;
    [SerializeField] private TMP_Text textPrefab;

    [SerializeField] private int maxWords = 10;
    [SerializeField] private float timeBetweenWords = 0.5f;

    private Queue<TextPiece> activeWords;
    private int totalWordsProcessed;
    private WordValidator validator;

    private void Awake()
    {
        validator = new WordValidator();
        activeWords = new Queue<TextPiece>();
    }

    private void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        SetSpawnPoint.Instance.UpdateSpawnPoints();

        StartCoroutine(spawner.EnterYourNameCourotine(
            maxWords,
            textPrefab,
            SetSpawnPoint.Instance.parrants,
            activeWords,
            timeBetweenWords,
            SetSpawnPoint.Instance.RightPos,
            SetSpawnPoint.Instance.LeftPos
        ));
    }

    private void Update()
    {
        CheckGameOver();
        ProcessInput();
    }

    private void ProcessInput()
    {
        if (activeWords.Count == 0) return;
        if (string.IsNullOrEmpty(UnityEngine.Input.inputString)) return;

        TextPiece currentWord = activeWords.Peek();

        foreach (char inputChar in UnityEngine.Input.inputString)
        {
            ProcessCharacter(inputChar, currentWord);

            if (currentWord.isComplete)
                break;
        }
    }

    private void ProcessCharacter(char inputChar, TextPiece word)
    {
        if (word.currentIndex >= word.tmpText.text.Length)
            return;

        bool isCorrect = validator.ValidateCharacter(inputChar, word);

        if (isCorrect)
        {
            WordBrush.SetCorrectColor(word.currentIndex, word);
        }
        else
        {
            WordBrush.SetIncorrectColor(word.currentIndex, word);
        }

        word.IncrementIndex();

        if (word.currentIndex >= word.tmpText.text.Length)
        {
            CompleteWord(word);
        }
    }

    private void CompleteWord(TextPiece word)
    {
        word.isComplete = true;

        if (activeWords.Count > 0)
        {
            activeWords.Dequeue();
            EvaluateWordAccuracy(word);
        }

        validator.Reset();
        totalWordsProcessed++;
    }

    private void EvaluateWordAccuracy(TextPiece word)
    {
        float accuracy = validator.GetAccuracy(word.tmpText.text.Length);
        float completionRatio = (float)word.currentIndex / word.tmpText.text.Length;

        Debug.Log($"Word: {word.tmpText.text}");

        if (completionRatio >= MIN_COMPLETION_THRESHOLD && accuracy >= MIN_ACCURACY_THRESHOLD)
        {
            HandleCorrectWord(word);
        }
    }

    private void HandleCorrectWord(TextPiece word)
    {
        WordBrush.SetDefaultColor(word);
        SpeedOfText.Instance.SpeedUp();
        CorrectWordsCount.Instance.correctWords++;

        StartCoroutine(mover.MoveFromScreenWinnigScenario(
            word,
            SetSpawnPoint.Instance.TopPos
        ));
    }

    private void OnWordMissed(TextPiece word)
    {
        if (activeWords.Count > 0 && activeWords.Peek() == word)
        {
            activeWords.Dequeue();
        }

        word.isComplete = false;
        validator.Reset();
        totalWordsProcessed++;
    }

    private void CheckGameOver()
    {
        if (totalWordsProcessed >= maxWords && activeWords.Count == 0)
        {
            EventManagerPuzzle.GameOverInv();
        }
    }

    private void OnEnable()
    {
        EventManagerPuzzle.OnMiss += OnWordMissed;
    }

    private void OnDisable()
    {
        EventManagerPuzzle.OnMiss -= OnWordMissed;
    }
}

