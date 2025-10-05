using UnityEngine;

[System.Serializable]
public class ItemOwnershipCondition : IConditionToBuy
{
    [SerializeField] private string requiredItemID;

    public ItemOwnershipCondition(string itemID)
    {
        requiredItemID = itemID;
    }

    public bool IsMet()
    {
        return ClickerShopManager.Instance.IsItemPurchased(requiredItemID);
    }

    public string GetRequirementDescription()
    {
        var item = ClickerShopManager.Instance.GetShopItem(requiredItemID);
        //Debug.Log($" item {item} title {item.Title} id {item.ItemID}");
        return $"œŒ“–≤¡ÕŒ:\n{item?.Title ?? requiredItemID}";
    }
}