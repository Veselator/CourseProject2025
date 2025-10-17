using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CourotineSpawner : MonoBehaviour
{
    [SerializeField] private CourotineWordMover wordMover;
    [SerializeField] private TypingGameplay _typingGameplay;
    [SerializeField] private SetSpawnPoint _setSpawnPoint;

    public IEnumerator EnterYourNameCourotine(int maxWords, TMP_Text text, Transform parrants, Queue<TextPiece> que, float timeBetweenWords, RectTransform rightPos, RectTransform leftPos, Action<TextPiece> OnWordChanged)
    {
        for (int k = 0; k < maxWords; ++k)
        {
            TextPiece textTest = new TextPiece();
            textTest.tmpText = Instantiate(text, parrants);
            //textTest.tmpText.gameObject.GetComponent<WordVIsualSelection>().Init(_typingGameplay);
            textTest.tmpText.text = GetWord.GetRandomWord();
            que.Enqueue(textTest);
            StartCoroutine(wordMover.MoveWord(textTest, rightPos, leftPos, _setSpawnPoint.borderOffsetX));

            if(_typingGameplay.ActiveWordsCount == 1)
            {
                yield return null;
                OnWordChanged?.Invoke(textTest);
            }

            yield return new WaitForSeconds(timeBetweenWords);
        }
    }
}
