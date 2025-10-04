using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterHandler : MonoBehaviour
{
    [SerializeField] private ClickerBooster _booster;
    [SerializeField] private GameObject currentGraphicPrefab;
    private UIBooster _UIBooster;
    public GameObject CurrentPrefab => currentGraphicPrefab;
    private ClickerManager _clickerManager;

    // Ленивая инициализация
    private ClickerManager ClickerManager
    {
        get
        {
            if (_clickerManager == null)
                _clickerManager = ClickerManager.Instance;
            return _clickerManager;
        }
    }

    private int currentNumOfUpgrades = 0;
    public int CurrentNumOfUpgrades => currentNumOfUpgrades;
    public bool IsBought { get; private set; } = false;

    // Формула расчёта текущей цены для апгрейда
    // Возможная оптимизация: кешировать значения PriceToUpgrade и CurrentIncomePerTick
    public float PriceToUpgrade => _booster.basePriceForUnit * Mathf.Pow(_booster.priceScalerFactor, currentNumOfUpgrades) * ClickerManager.PriceFactor;
    public float PriceToUnlock => _booster.priceToUnlock;
    public float CurrentIncomePerTick => currentNumOfUpgrades * _booster.incomePerUnit;
    public string Title => _booster.title;
    public bool IsAvailableToUpgrade => IsBought && (ClickerManager.IsAffordable(PriceToUpgrade));
    public bool IsAvailableToBuy => ClickerManager.IsAffordable(PriceToUnlock);

    public Action OnBoosterBought;
    public Action OnBoosterUpgraded;

    private void Awake()
    {
        _UIBooster = GetComponent<UIBooster>();
    }

    public bool TryToBuy()
    {
        if (IsBought) return false;

        if (_clickerManager.IsAffordable(_booster.priceToUnlock))
        {
            _clickerManager.ChangeMoney(-_booster.priceToUnlock);
            IsBought = true;

            OnBoosterBought?.Invoke();
            return true;
        }

        return false;
    }

    public bool TryToUpgrade()
    {
        if (IsAvailableToUpgrade)
        {
            _clickerManager.ChangeMoney(-PriceToUpgrade);
            currentNumOfUpgrades++;

            OnBoosterUpgraded?.Invoke();
            return true;
        }

        return false;
    }

    public void ShowAnimation()
    {
        // КОд анимации
        if (_UIBooster == null) _UIBooster = GetComponent<UIBooster>();
        _UIBooster.ShowAnimation();
    }
}
