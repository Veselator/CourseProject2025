using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueTextWriter : MonoBehaviour
{
    // Скрипт, которые отвечает за отображение диалога
    [SerializeField] private TextMeshProUGUI _dialogueText;
    private bool isPlayingAnimation = false;

    public event Action OnWritingEnded;
    public void Write(DialogueNodeSO node)
    {
        if (isPlayingAnimation)
        {
            StopAllCoroutines();
            isPlayingAnimation = false;
        }

        StartCoroutine(Writing(node));
    }

    private IEnumerator Writing(DialogueNodeSO node)
    {
        isPlayingAnimation = true;

        string allText = node.Text;

        _dialogueText.text = allText;
        _dialogueText.maxVisibleCharacters = 0;

        int totalCharacters = allText.Length;
        int currentTypingConfigId = 0;
        CharacterTypingSpeedConfig currentTypingConfig = node.TypingConfig[currentTypingConfigId];

        for (int i = 1; i <= totalCharacters; i++)
        {
            _dialogueText.maxVisibleCharacters = i;

            if(Char.IsLetter(allText[i - 1])) yield return new WaitForSeconds(currentTypingConfig.TypingSpeed);

            // Если подошли к концу - меняем currentTypingConfig
            if (i == currentTypingConfig.IDTo)
            {
                yield return new WaitForSeconds(currentTypingConfig.DelayAfter);
                currentTypingConfigId++;

                // Если вышли за границы массива
                if (currentTypingConfigId >= node.TypingConfig.Length) continue;

                currentTypingConfig = node.TypingConfig[currentTypingConfigId];
            }
        }

        yield return new WaitForSeconds(node.DelayAfter);
        isPlayingAnimation = false;

        OnWritingEnded?.Invoke();
    }
}
