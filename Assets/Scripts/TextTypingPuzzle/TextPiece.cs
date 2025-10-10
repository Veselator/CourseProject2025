using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextPiece
{
    public TMP_Text tmpText;
    public bool isComplete = false;
    public int currentIndex = 0;


    public void IncrementIndex() 
    {
        currentIndex++;
    }
}
