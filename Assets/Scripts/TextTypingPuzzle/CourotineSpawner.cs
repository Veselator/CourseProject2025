using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CourotineSpawner : MonoBehaviour
{
    [SerializeField] private CourotineWordMover wordMover;
    public IEnumerator EnterYourNameCourotine(int maxWords, TMP_Text text, Transform parrants, Queue<TextPiece> que, float timeBetweenWords, RectTransform rightPos, RectTransform leftPos)
    {
        for (int k = 0; k < maxWords; ++k)
        {
            TextPiece textTest = new TextPiece();
            textTest.tmpText = Instantiate(text, parrants);
            textTest.tmpText.text = GetWord.GetRandomWord();
            que.Enqueue(textTest);
            StartCoroutine(wordMover.MoveWord(textTest, rightPos, leftPos));

            yield return new WaitForSeconds(timeBetweenWords);
        }
    }
}
