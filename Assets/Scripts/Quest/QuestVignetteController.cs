using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class QuestVignetteController : MonoBehaviour
{
    private QuestVignetteAnimator _animator;
    [SerializeField] private float _currentVignetteValue = 0.355f;
    private float _startVignetteValue;
    private float CurrentVegnetteValue
    {
        get => _currentVignetteValue;
        set
        {
            _currentVignetteValue = value;
            if(_animator != null) _animator.AnimateValue(_currentVignetteValue);
        }
    }

    [SerializeField] private BorderValueOfTime[] borderValuesOfTime;
    private QuestTimerManager _timerManager;

    private void Start()
    {
        _startVignetteValue = _currentVignetteValue;

        _animator = GetComponent<QuestVignetteAnimator>();
        _animator.SetValueImmediate(CurrentVegnetteValue);
        _timerManager = QuestTimerManager.Instance;

        _timerManager.OnTimerChanged += CheckTime;
        _timerManager.OnTimerReset += ResetVignette;
    }

    private void OnDestroy()
    {
        _timerManager.OnTimerChanged -= CheckTime;
        _timerManager.OnTimerReset -= ResetVignette;
    }

    private void ResetVignette()
    {
        CurrentVegnetteValue = _startVignetteValue;
    }

    private void CheckTime(float time)
    {
        float newValue = _currentVignetteValue;

        foreach (var border in borderValuesOfTime)
        {
            // С учётом того, что массив не отсортирован по времени
            if (time <= border.time)
            {
                newValue = border.vignetteValue;
            }
        }

        if (newValue == _currentVignetteValue) return;
        CurrentVegnetteValue = newValue;
    }
}

[Serializable]
public struct BorderValueOfTime
{
    public float time;
    public float vignetteValue;
}
