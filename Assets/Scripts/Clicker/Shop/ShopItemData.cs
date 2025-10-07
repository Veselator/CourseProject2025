using UnityEngine;

[CreateAssetMenu(fileName = "ShopItem", menuName = "Clicker/Shop Item")]
public class ShopItemData : ScriptableObject
{
    [Header("Основные данные")]
    public string itemID;
    public string title;
    [TextArea(2, 4)]
    public string description;
    public Sprite icon;
    public float price;

    [Header("Категория")]
    public ShopItemCategory category;

    [Header("Эффекты")]
    public ShopItemEffect[] effects;

    [Header("Условия покупки")]
    public ConditionType[] conditions;
    public string[] requiredItemIDs;
}

public enum ShopItemCategory
{
    ClickPower,
    Cosmetics,
    Special
}

[System.Serializable]
public class ShopItemEffect
{
    public EffectType type;
    public float value;
}

public enum EffectType
{
    ClickPowerMultiplier,
    ClickPowerBonus,
    UnlockCosmetic,
    ChangePriceFactor,
    EndGame
}

public enum ConditionType
{
    None,
    RequireItem,
    RequireAchievement
}