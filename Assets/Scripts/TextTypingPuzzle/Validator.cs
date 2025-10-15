using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordValidator
{
    private int correctLettersCount;

    public bool ValidateCharacter(char input, TextPiece word)
    {
        string targetText = word.tmpText.text;
        int currentIndex = word.currentIndex;

        if (currentIndex >= targetText.Length)
            return false;

        bool isCorrect = char.ToLower(input) == char.ToLower(targetText[currentIndex]);

        if (isCorrect)
        {
            correctLettersCount++;
        }

        return isCorrect;
    }

    public float GetAccuracy(int totalLetters)
    {
        if (totalLetters == 0) return 0f;
        return (float)correctLettersCount / totalLetters * 100f;
    }

    public void Reset()
    {
        correctLettersCount = 0;
    }
}
