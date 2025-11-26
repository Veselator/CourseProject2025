public class WordValidator
{
    // Класс для проверки правильности написания сиволов в слове

    private int correctLettersCount;
    public int CorrectLettersCount => correctLettersCount;

    public bool ValidateCharacter(char input, TextPiece word)
    {
        string targetText = word.tmpText.text;
        int currentIndex = word.currentIndex;

        if (currentIndex >= targetText.Length)
            return false;

        bool isCorrect = LanguageValidator.ValidateChar(input) == char.ToLower(targetText[currentIndex]);

        if (isCorrect)
        {
            correctLettersCount++;
        }

        return isCorrect;
    }

    public float GetAccuracy(int totalLetters)
    {
        if (totalLetters == 0) return 0f;
        return (float)correctLettersCount / totalLetters;
    }

    public void Reset()
    {
        correctLettersCount = 0;
    }
}
