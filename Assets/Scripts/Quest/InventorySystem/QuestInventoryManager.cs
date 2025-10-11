using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestInventoryManager : MonoBehaviour
{
    private List<QuestItem> _inventory = new List<QuestItem>();
    public List<QuestItem> Inventory => _inventory;

    public static QuestInventoryManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void AddItem(QuestItem item)
    {
        _inventory.Add(item);
    }

    public bool DoesHaveItem(QuestItem item)
    {
        return _inventory.Contains(item);
    }

    public bool DoesHaveAnyItem()
    {
        return _inventory.Count > 0;
    }

    public QuestItem GetItem(int index)
    {
        return _inventory[index];
    }

    public void RemoveItem(int index)
    {
        _inventory.RemoveAt(index);
    }

    public void RemoveItem(QuestItem item)
    {
        if (DoesHaveItem(item))
        {
            _inventory.Remove(item);
        }
    }
}
