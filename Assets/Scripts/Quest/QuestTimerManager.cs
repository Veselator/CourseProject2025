using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTimerManager : MonoBehaviour
{
    private const float TIME_PER_TICK = 1.1f;
    private float _timer = 0f;
    private float _nextTickTime;
    [SerializeField] private float[] timersPerLevel;

    public static QuestTimerManager Instance {  get; private set; }

    public event Action<float> OnTimerChanged;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        _nextTickTime = Time.time + TIME_PER_TICK;
    }

    private void Update()
    {
        if (Time.time >= _nextTickTime)
        {
            _timer -= 1f;
            OnTimerChanged?.Invoke(_timer);
            _nextTickTime = Time.time + TIME_PER_TICK;
        }
    }
}
