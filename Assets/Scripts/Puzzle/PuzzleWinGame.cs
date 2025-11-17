using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleWinGame : MonoBehaviour
{
    public static PuzzleWinGame Instance { get; private set; }

    // Реализуем
    private void Awake()
    {
        Instance = this;
    }

    public void WinGame()
    {
        GameSceneManager.LoadNextScene();
    }
}
