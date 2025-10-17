using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public static class EventManagerPuzzle
{
    public static Action gameOverAction;
    public static Action<TextPiece> OnMiss;

    public static void GameOverInv() 
    { 
        gameOverAction?.Invoke();
    }

    public static void OnMissInv(TextPiece text) 
    { 
        OnMiss?.Invoke(text);
    }   
}
