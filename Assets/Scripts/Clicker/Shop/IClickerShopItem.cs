using System;

public interface IClickerShopItem
{
    string ItemID { get; }
    string Title { get; }
    string Description { get; }
    float Price { get; }
    bool IsBought { get; }
    bool IsAvailable { get; } // Доступен ли для покупки (условия выполнены)
    bool IsAffordable { get; } // Хватает ли денег

    event Action<IClickerShopItem> OnItemPurchased;
    event Action<IClickerShopItem> OnAvailabilityChanged;

    bool TryToPurchase();
    void ApplyEffect();
}