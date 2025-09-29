using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootemUpScoreManager : MonoBehaviour
{
    public static ShootemUpScoreManager Instance { get; private set; }
    public int Score { get; private set; }

    public Action<int> AddScoreAction;
    public Action OnMoneyChanged;
    
    private void Awake()
    {
        if(Instance == null) Instance = this;
    }

    private void Start()
    {
        AddScoreAction += AddScore;
    }

    private void OnDestroy()
    {
        AddScoreAction -= AddScore;
    }

    private void AddScore(int score)
    {
        Score += score;
        OnMoneyChanged?.Invoke();
    }
}
