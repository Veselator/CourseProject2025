using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TypingGameplay : MonoBehaviour
{
    private const float MIN_ACCURACY_THRESHOLD = 60f;
    private const float MIN_COMPLETION_THRESHOLD = 0.5f;
    private const float ACCURACY_TO_WIN_THRESHOLD = 0.7f; // Порог для победы

    [SerializeField] private CourotineSpawner spawner;
    [SerializeField] private CourotineWordMover mover;
    [SerializeField] private TMP_Text textPrefab;

    [SerializeField] private int maxWords = 10;
    [SerializeField] private float timeBetweenWords = 0.5f;

    private Queue<TextPiece> activeWords;
    public int ActiveWordsCount => activeWords.Count;
    private WordValidator validator;
    private GameProgress gameProgress;
    private bool isGameActive;
    public float CurrentAccuracy => gameProgress.GetOverallAccuracy();

    public event Action<int, char> OnCharacterTyped;
    public event Action<int, char> OnCharacterCorrectTyped;
    public event Action<int, char> OnCharacterIncorrectTyped;
    public event Action<TextPiece> OnWordChanged;
    public string CurrentWord => activeWords.Count > 0 ? activeWords.Peek().tmpText.text : string.Empty;

    private void Awake()
    {
        validator = new WordValidator();
        activeWords = new Queue<TextPiece>();
        gameProgress = new GameProgress();
    }

    private void Start()
    {
        EventManagerPuzzle.OnMiss += OnWordMissed;
        InitializeGame();
    }

    private void OnDestroy()
    {
        EventManagerPuzzle.OnMiss -= OnWordMissed;
    }

    private void InitializeGame()
    {
        isGameActive = true;
        SetSpawnPoint.Instance.UpdateSpawnPoints();

        StartCoroutine(spawner.EnterYourNameCourotine(
            maxWords,
            textPrefab,
            SetSpawnPoint.Instance.parrants,
            activeWords,
            timeBetweenWords,
            SetSpawnPoint.Instance.RightPos,
            SetSpawnPoint.Instance.LeftPos, OnWordChanged
        ));
    }

    private void Update()
    {
        if (!isGameActive) return;

        ProcessInput();
        CheckGameEnd();
    }

    private void ProcessInput()
    {
        if (activeWords.Count == 0) return;
        if (string.IsNullOrEmpty(Input.inputString)) return;

        TextPiece currentWord = activeWords.Peek();

        foreach (char inputChar in Input.inputString)
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
        int currentIndex = word.currentIndex;

        if (isCorrect)
        {
            WordBrush.SetCorrectColor(currentIndex, word);
            OnCharacterCorrectTyped?.Invoke(currentIndex, inputChar);
            gameProgress.RegisterCorrectChar();
        }
        else
        {
            WordBrush.SetIncorrectColor(currentIndex, word);
            OnCharacterIncorrectTyped?.Invoke(currentIndex, inputChar);
            gameProgress.RegisterIncorrectChar();
        }

        OnCharacterTyped?.Invoke(currentIndex,inputChar);
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

            if (activeWords.Count > 0) OnWordChanged?.Invoke(activeWords.Peek());
            else OnWordChanged?.Invoke(null);
        }

        validator.Reset();
        gameProgress.IncrementProcessedWords();
    }

    private void EvaluateWordAccuracy(TextPiece word)
    {
        float accuracy = validator.GetAccuracy(word.tmpText.text.Length);
        float completionRatio = (float)word.currentIndex / word.tmpText.text.Length;

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

        if (activeWords.Count > 0) OnWordChanged?.Invoke(activeWords.Peek());
        else OnWordChanged?.Invoke(null);

        word.isComplete = false;
        validator.Reset();
        gameProgress.IncrementProcessedWords();
    }

    private void CheckGameEnd()
    {
        if (!gameProgress.IsGameComplete(maxWords, activeWords.Count))
            return;

        isGameActive = false;
        float overallAccuracy = gameProgress.GetOverallAccuracy();

        Debug.Log($"Game ended. Overall accuracy: {overallAccuracy:F2}%");

        if (overallAccuracy >= ACCURACY_TO_WIN_THRESHOLD)
        {
            GameSceneManager.LoadNextScene();
        }
        else
        {
            EventManagerPuzzle.GameOverInv();
        }
    }
}

public class GameProgress
{
    private int totalWordsProcessed;
    private int correctCharsTyped;
    private int incorrectCharsTyped;

    public void IncrementProcessedWords()
    {
        totalWordsProcessed++;
    }

    public void RegisterCorrectChar()
    {
        correctCharsTyped++;
    }

    public void RegisterIncorrectChar()
    {
        incorrectCharsTyped++;
    }

    public bool IsGameComplete(int maxWords, int activeWordsCount)
    {
        return totalWordsProcessed >= maxWords && activeWordsCount == 0;
    }

    public float GetOverallAccuracy()
    {
        int totalChars = correctCharsTyped + incorrectCharsTyped;

        if (totalChars == 0)
            return 0f;

        return (correctCharsTyped / (float)totalChars);
    }

    public int GetTotalWordsProcessed() => totalWordsProcessed;
    public int GetCorrectChars() => correctCharsTyped;
    public int GetIncorrectChars() => incorrectCharsTyped;
}