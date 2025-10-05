using System;

public interface IClickerShopItem
{
    string ItemID { get; }
    string Title { get; }
    string Description { get; }
    float Price { get; }
    bool IsBought { get; }
    bool IsAvailable { get; } // �������� �� ��� ������� (������� ���������)
    bool IsAffordable { get; } // ������� �� �����

    event Action<IClickerShopItem> OnItemPurchased;
    event Action<IClickerShopItem> OnAvailabilityChanged;

    bool TryToPurchase();
    void ApplyEffect();
}