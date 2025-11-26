using System;
using UnityEngine;

public class BoosterHandler : MonoBehaviour
{
    [SerializeField] private ClickerBooster _linkedBooster;
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

    private int _currentLevel = 0;
    public int CurrentLevel => _currentLevel;
    public int MaxLevel => _linkedBooster.maxLevel;
    public bool IsReachedMaxLevel => CurrentLevel >= MaxLevel;
    public bool IsBought { get; private set; } = false;

    // Формула расчёта текущей цены для апгрейда
    // Возможная оптимизация: кешировать значения PriceToUpgrade и CurrentIncomePerTick
    public float PriceToUpgrade => _linkedBooster.basePriceForUnit * Mathf.Pow(_linkedBooster.priceScalerFactor, _currentLevel) * ClickerManager.PriceFactor;
    public float PriceToUnlock => _linkedBooster.priceToUnlock * ClickerManager.PriceFactor;
    public float CurrentIncomePerTick => _currentLevel * _linkedBooster.incomePerUnit;
    public string Title => _linkedBooster.title;
    public bool IsAvailableToUpgrade => IsBought && ClickerManager.IsAffordable(PriceToUpgrade) && _currentLevel < _linkedBooster.maxLevel;
    public bool IsAvailableToBuy => ClickerManager.IsAffordable(PriceToUnlock);

    public Action OnBoosterBought;
    public Action OnBoosterUpgraded;
    public Action OnFailedToDoAction;

    private void Awake()
    {
        _UIBooster = GetComponent<UIBooster>();
    }

    public bool TryToBuy()
    {
        if (IsBought) return false;

        if (_clickerManager.IsAffordable(_linkedBooster.priceToUnlock))
        {
            _clickerManager.ChangeMoney(-_linkedBooster.priceToUnlock);
            IsBought = true;

            OnBoosterBought?.Invoke();
            return true;
        }

        OnFailedToDoAction?.Invoke();
        return false;
    }

    public bool TryToUpgrade()
    {
        if (IsAvailableToUpgrade)
        {
            _clickerManager.ChangeMoney(-PriceToUpgrade);
            _currentLevel++;

            OnBoosterUpgraded?.Invoke();
            return true;
        }

        OnFailedToDoAction?.Invoke();
        return false;
    }

    public void ShowAnimation()
    {
        // КОд анимации появления
        if (_UIBooster == null) _UIBooster = GetComponent<UIBooster>();
        _UIBooster.ShowAnimation();
    }
}
