using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterHandler : MonoBehaviour
{
    [SerializeField] private ClickerBooster _booster;
    private ClickerManager _clickerManager;
    private int currentNumOfUpgrades = 0;
    public bool IsBought { get; private set; } = false;

    // Формула расчёта текущей цены для апгрейда
    // Возможная оптимизация: кешировать значения PriceToUpgrade и CurrentIncomePerTick
    private float PriceToUpgrade => _booster.basePriceForUnit * Mathf.Pow(_booster.priceScalerFactor, currentNumOfUpgrades) * _clickerManager.PriceFactor;
    public float CurrentIncomePerTick => currentNumOfUpgrades * _booster.incomePerUnit;

    public Action OnBoosterBought;
    public Action OnBoosterUpgraded;

    private void Start()
    {
        _clickerManager = ClickerManager.Instance;
    }

    public void TryToBuy()
    {
        if (IsBought) return;

        if (_clickerManager.IsAffordable(_booster.priceToUnlock))
        {
            _clickerManager.ChangeMoney(-_booster.priceToUnlock);
            IsBought = true;

            OnBoosterBought?.Invoke();
        }
    }

    public void TryToUpgrade()
    {
        if (_clickerManager.IsAffordable(PriceToUpgrade))
        {
            _clickerManager.ChangeMoney(-PriceToUpgrade);
            currentNumOfUpgrades++;

            OnBoosterUpgraded?.Invoke();
        }
    }
}
