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
    [SerializeField] private CourotineSpawner spawner;
    [SerializeField] private CourotineWordMover mover;
    [SerializeField] private TMP_Text text;
    [SerializeField] private int maxWords = 10;
  
   
   
    [SerializeField] private float timeBetweenWords = 0.5f;
   


    private int correctLetters = 0;
    private int totalWordsTyped = 0;

   
    private Queue<TextPiece> que;

   
  

    private void GetBasicParamas() 
    {
        Debug.Log("Returned to basic");
        correctLetters = 0;
   
    }
   
 
    private void CheckCorrection(char c, TextPiece text1)
    {
       
        string str = text1.tmpText.text;
        
        if (text1.currentIndex >= str.Length)
        {
            
            return;
        }


        if (char.ToLower(c) == char.ToLower(str[text1.currentIndex]))
        {
            WordBrush.SetCorrectColor(text1.currentIndex, text1);
            correctLetters++;
        }
        else
        {
            WordBrush.SetIncorrectColor(text1.currentIndex, text1);
        }

        text1.IncrementIndex();
        if (text1.currentIndex >= str.Length)
        {
            Debug.Log("Word completed!");
          
            text1.isComplete = true;

            if (que.Count > 0)
            {
                que.Dequeue();
                GetAccuracy(text1.currentIndex, str, text1);
                Debug.Log("Den");
            }

            GetBasicParamas();
            return;
        }
    }

    private void Gameplay(TextPiece text1)
    {
        

        foreach (char c in UnityEngine.Input.inputString)
        {
           
            CheckCorrection(c, text1);
        }
    }
    private void GetAccuracy(int currentIndex, string str, TextPiece text1) 
    {
        if (currentIndex == 0)
        {
            return;
        }
        float accuracy = str.Length > 0 ? (float)correctLetters / str.Length * 100f : 0f;
        if (currentIndex > str.Length/2 && accuracy > 60f) 
        {
            WordBrush.SetDefaultColor(text1);
            SpeedOfText.Instance.SpeedUp();
            StartCoroutine(mover.MoveFromScreenWinnigScenario(text1, SetSpawnPoint.Instance.TopPos));
            CorrectWordsCount.Instance.correctWords++;
         



        }
        
        totalWordsTyped++;


    }
  
 
    private void OnMiss(TextPiece text1) 
    {
        if (que.Count > 0)
            que.Dequeue(); 
        text1.isComplete = false;
        totalWordsTyped++;
        GetBasicParamas();
    }
    private void OnDestroy()
    {
        EventManagerPuzzle.OnMiss -= OnMiss;
    }
    private void Start()
    {
        que = new Queue<TextPiece>();


        SetSpawnPoint.Instance.UpdateSpawnPoints();
        EventManagerPuzzle.OnMiss += OnMiss;

        
        StartCoroutine(spawner.EnterYourNameCourotine(
            maxWords,
            text,
             SetSpawnPoint.Instance.parrants,
            que,
            timeBetweenWords,
            SetSpawnPoint.Instance.RightPos,
            SetSpawnPoint.Instance.LeftPos

        ));
    }
  
    private void Update()
    {
        if (totalWordsTyped >= maxWords && que.Count <= 0) 
        {
            EventManagerPuzzle.GameOverInv();
        }
        if (que.Count > 0)
        {
            TextPiece CurrentWord = que.Peek();
            Gameplay(CurrentWord);
        }
    }
}

