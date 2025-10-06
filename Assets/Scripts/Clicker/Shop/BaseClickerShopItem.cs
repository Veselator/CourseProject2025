using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseClickerShopItem : MonoBehaviour, IClickerShopItem
{
    private string itemID; // ID для связывания с данными

    [Header("Данные предмета")]
    [SerializeField] protected ShopItemData itemData; // Может быть назначено в инспекторе или программно

    private ClickerManager _clickerManager;
    private List<IConditionToBuy> _conditions = new List<IConditionToBuy>();
    private bool _isBought = false;
    private bool _isInitialized = false;

    // Свойства
    public string ItemID => itemData?.itemID ?? itemID;
    public string Title => itemData?.title ?? "Unknown Item";
    public string Description => itemData?.description ?? "";
    public float Price => itemData != null ? itemData.price : 0; // Важно: в магазине фактор цен не учитываем
    public bool IsBought => _isBought;
    public bool IsAvailable => CheckAvailability();
    public bool IsAffordable => ClickerManager.Instance.IsAffordable(Price);
    public Sprite Icon => itemData?.icon;
    public ShopItemCategory Category => itemData?.category ?? ShopItemCategory.Special;

    public event Action<IClickerShopItem> OnItemPurchased;
    public event Action<IClickerShopItem> OnAvailabilityChanged;

    // НОВЫЙ МЕТОД - для инициализации данными из менеджера
    public void InitializeWithData(ShopItemData data)
    {
        if (_isInitialized)
        {
            Debug.LogWarning($"Предмет {gameObject.name} уже инициализирован!");
            return;
        }

        itemData = data;
        itemID = data.itemID;
        InitializeConditions();
        _isInitialized = true;

        // Если Awake уже был вызван, подписываемся на события
        if (_clickerManager != null)
        {
            SubscribeToEvents();
        }
    }

    // Метод для получения ID (используется при автопоиске)
    public string GetItemID()
    {
        return string.IsNullOrEmpty(itemID) ? gameObject.name : itemID;
    }

    protected virtual void Start()
    {
        _clickerManager = ClickerManager.Instance;

        // Если данные уже назначены в инспекторе, инициализируемся
        if (itemData != null)
        {
            InitializeWithData(itemData);
            SubscribeToEvents();
        }
    }

    private void SubscribeToEvents()
    {
        _clickerManager.OnMoneyChanged += OnMoneyChanged;

        if (ClickerShopManager.Instance != null)
        {
            ClickerShopManager.Instance.OnAnyItemPurchased += OnAnyItemPurchased;
        }
    }

    protected virtual void OnDestroy()
    {
        if (_clickerManager != null)
        {
            _clickerManager.OnMoneyChanged -= OnMoneyChanged;
        }

        if (ClickerShopManager.Instance != null)
        {
            ClickerShopManager.Instance.OnAnyItemPurchased -= OnAnyItemPurchased;
        }
    }

    private void InitializeConditions()
    {
        _conditions.Clear();

        if (itemData == null) return;

        foreach (var conditionType in itemData.conditions)
        {
            switch (conditionType)
            {
                case ConditionType.RequireItem:
                    foreach (var requiredID in itemData.requiredItemIDs)
                    {
                        _conditions.Add(new ItemOwnershipCondition(requiredID));
                    }
                    break;
            }
        }
    }

    private bool CheckAvailability()
    {
        if (_isBought) return false;

        foreach (var condition in _conditions)
        {
            if (!condition.IsMet())
                return false;
        }

        return true;
    }

    public virtual bool TryToPurchase()
    {
        if (_isBought) return false;
        if (!IsAvailable) return false;
        if (!IsAffordable) return false;

        _clickerManager.ChangeMoney(-Price);
        _isBought = true;

        ApplyEffect();

        OnItemPurchased?.Invoke(this);
        ClickerShopManager.Instance.RegisterPurchase(this);

        return true;
    }

    public abstract void ApplyEffect();

    public List<string> GetUnmetRequirements()
    {
        var requirements = new List<string>();

        foreach (var condition in _conditions)
        {
            if (!condition.IsMet())
            {
                requirements.Add(condition.GetRequirementDescription());
            }
        }

        return requirements;
    }

    private void OnMoneyChanged(float newAmount)
    {
        OnAvailabilityChanged?.Invoke(this);
    }

    private void OnAnyItemPurchased(IClickerShopItem item)
    {
        if (item != this)
        {
            OnAvailabilityChanged?.Invoke(this);
        }
    }
}