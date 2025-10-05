using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ClickerShopManager : MonoBehaviour
{
    public static ClickerShopManager Instance { get; private set; }

    [Header("Существующие предметы в сцене")]
    [SerializeField] private BaseClickerShopItem[] existingShopItems; // Массив уже существующих предметов

    private Dictionary<string, BaseClickerShopItem> _shopItems = new Dictionary<string, BaseClickerShopItem>();
    private HashSet<string> _purchasedItemIDs = new HashSet<string>();

    public event Action<IClickerShopItem> OnAnyItemPurchased;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        InitializeShop();
    }

    private void InitializeShop()
    {
        //LoadPurchasedItems();
        LinkExistingItems();      // Связываем существующие предметы с данными
    }

    // НОВЫЙ МЕТОД - связывание существующих GameObject с данными
    private void LinkExistingItems()
    {

        // Связываем по индексу или по ID
        for (int i = 0; i < existingShopItems.Length; i++)
        {
            var shopItem = existingShopItems[i];

            if (shopItem != null)
            {
                // Инициализируем предмет данными
                //shopItem.InitializeWithData(itemData);

                // Добавляем в словарь
                _shopItems[shopItem.ItemID] = shopItem;

                // Инициализируем UI компонент
                ClickerUIShopItem uiShopItem = shopItem.GetComponent<ClickerUIShopItem>();
                if (uiShopItem != null)
                {
                    uiShopItem.Initialize(shopItem);
                }
            }
        }
    }

    public void RegisterPurchase(IClickerShopItem item)
    {
        _purchasedItemIDs.Add(item.ItemID);
        OnAnyItemPurchased?.Invoke(item);
    }

    public bool IsItemPurchased(string itemID)
    {
        return _purchasedItemIDs.Contains(itemID);
    }

    public IClickerShopItem GetShopItem(string itemID)
    {
        return _shopItems.ContainsKey(itemID) ? _shopItems[itemID] : null;
    }

    public List<IClickerShopItem> GetItemsByCategory(ShopItemCategory category)
    {
        return _shopItems.Values
            .Where(item => item.Category == category)
            .Cast<IClickerShopItem>()
            .ToList();
    }

    public List<IClickerShopItem> GetAffordableItems()
    {
        return _shopItems.Values
            .Where(item => !item.IsBought && item.IsAvailable && item.IsAffordable)
            .Cast<IClickerShopItem>()
            .ToList();
    }
}