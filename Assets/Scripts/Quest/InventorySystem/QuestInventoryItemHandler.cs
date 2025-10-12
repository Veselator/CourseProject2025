using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestInventoryItemHandler : MonoBehaviour
{
    public QuestInventoryItem LinkedItem { get; private set; }
    private QuestInventoryManager _questInventoryManager;
    public int id = 0;

    public void Init(QuestInventoryManager questInvManager, QuestInventoryItem linkedItem, int id)
    {
        _questInventoryManager = questInvManager;
        LinkedItem = linkedItem;
        GetComponent<Image>().sprite = LinkedItem.itemIcon;

        this.id = id;
    }

    public void HandleClick()
    {
        _questInventoryManager.SelectItem(id);
    }
}
