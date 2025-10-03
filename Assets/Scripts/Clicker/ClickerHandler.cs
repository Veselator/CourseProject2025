using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

public class ClickerHandler : MonoBehaviour
{
    // Реализует бизнес-логику ClickerManager

    private ClickerManager _clickerManager;
    [SerializeField] private BoosterHandler[] _boosterHandlers;

    // Время одного тика
    // За один тик игрок поулчает доход от всех бустеров
    private const float timePerTick = 3f;
    private float timer = 0f;

    private void Awake()
    {
        Init(new ClickerManager());
    }

    private void Update()
    {
        UpdateTimer();
        CheckTick();
    }

    private void Init(ClickerManager clickerManager)
    {
        _clickerManager = clickerManager;
    }

    private void UpdateTimer()
    {
        timer += Time.deltaTime;
    }

    private void CheckTick()
    {
        if (timer > timePerTick)
        {
            timer = 0f;
            ApplyBoosters();
        }
    }

    private void ApplyBoosters()
    {
        if (_boosterHandlers.Length == 0) return;
        
        foreach (var boosterHandler in _boosterHandlers)
        {
            // Все бустеры у нас расположены по прядку
            // Если кто-то один не куплен - значит, и все бустеры дальше не куплены
            // Нет смысла дальше проверять
            if (!boosterHandler.IsBought) break;

            _clickerManager.ChangeMoney(boosterHandler.CurrentIncomePerTick);
        }
    }

    public void ProcessUserClick()
    {
        _clickerManager.ChangeMoney(_clickerManager.IncomePerClick);
    }
}
