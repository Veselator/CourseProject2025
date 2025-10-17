using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class CurrentWordUIVisual : MonoBehaviour
{
    [SerializeField] private TypingGameplay _typingGameplay;
    [SerializeField] private TMP_Text _currentWordText;

    private void Awake()
    {
        _typingGameplay.OnWordChanged += UpdateCurrentWordVisual;
        _typingGameplay.OnCharacterCorrectTyped += OnCharacterCorrectTyped;
        _typingGameplay.OnCharacterIncorrectTyped += OnCharacterIncorrectTyped;

        UpdateCurrentWordVisual("");
    }

    private void OnDestroy()
    {
        _typingGameplay.OnWordChanged -= UpdateCurrentWordVisual;
        _typingGameplay.OnCharacterCorrectTyped -= OnCharacterCorrectTyped;
        _typingGameplay.OnCharacterIncorrectTyped -= OnCharacterIncorrectTyped;
    }

    private void OnCharacterCorrectTyped(int currentIndex, char character)
    {
        Debug.Log("OnCharacterCorrectTyped");
        WordBrush.SetCorrectColor(currentIndex, _currentWordText);
    }

    private void OnCharacterIncorrectTyped(int currentIndex, char character)
    {
        Debug.Log("OnCharacterIncorrectTyped");
        WordBrush.SetIncorrectColor(currentIndex, _currentWordText);
    }

    private void UpdateCurrentWordVisual(TextPiece newWord)
    {
        Debug.Log("Updating current world visual");
        if (newWord == null)
        {
            _currentWordText.text = "";
            return;
        }

        _currentWordText.text = newWord.tmpText.text;
    }

    private void UpdateCurrentWordVisual(string newWord)
    {
        _currentWordText.text = newWord;
    }
}
