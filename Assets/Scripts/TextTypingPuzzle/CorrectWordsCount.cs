using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CorrectWordsCount : MonoBehaviour
{
   public static CorrectWordsCount Instance { get; private set; }

    public int correctWords = 0;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

    }
}
