using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupableItem : BaseItem
{
    [Header("Предмет, на который ссылаемся")]
    [SerializeField] private QuestInventoryItem itemData; // то, на какой предмет ссылаемся
    [Header("Действие, которое выполняется при подборе предмета")]
    [SerializeField] private QuestAction additionalActions;

    protected override void Start()
    {
        itemID = itemData.itemId; // Важно
        itemData.worldPickupPosition = transform.position;
        base.Start();
    }

    public override bool CanInteract() => true;

    public override void Interact()
    {
        QuestInventoryManager.Instance.AddItem(itemData);
        if (additionalActions != null) QuestActionProccessor.Instance.ProcessAction(additionalActions, this.gameObject);
        Destroy(gameObject);
    }
}
