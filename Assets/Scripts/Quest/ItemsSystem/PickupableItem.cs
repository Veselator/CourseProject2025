using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupableItem : BaseItem
{
    [SerializeField] private QuestInventoryItem itemData; // то, на какой предмет ссылаемся
    [SerializeField] private QuestAction additionalActions;

    private void Start()
    {
        itemData.itemId = itemID;
    }

    public override bool CanInteract() => true;

    public override void Interact()
    {
        QuestInventoryManager.Instance.AddItem(itemData);
        if (additionalActions != null) QuestActionProccessor.Instance.ProcessAction(additionalActions, this.gameObject);
        Destroy(gameObject);
    }
}
