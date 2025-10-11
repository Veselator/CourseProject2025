using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestInventoryItemHandler : MonoBehaviour
{
    public QuestInventoryItem LinkedItem { get; private set; }
    private QuestInventoryManager _questInventoryManager;
    public int id = 0;

    public void Init(QuestInventoryItem linkedItem, int id)
    {
        LinkedItem = linkedItem;
        GetComponent<Image>().sprite = LinkedItem.itemIcon;

        this.id = id;
    }

    private void OnMouseDown()
    {
        _questInventoryManager.SelectItem(id);
    }
}
