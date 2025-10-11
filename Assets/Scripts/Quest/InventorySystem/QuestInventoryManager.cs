using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestInventoryManager : MonoBehaviour
{
    private List<QuestInventoryItem> _inventory = new List<QuestInventoryItem>();
    public List<QuestInventoryItem> Inventory => _inventory;
    public int SelectedItemId { get; private set; } = -1;
    public QuestInventoryItem SelectedItem => SelectedItemId == -1 ? null : _inventory[SelectedItemId];
    public bool IsSelectedAnyItem => SelectedItemId != -1;

    public static QuestInventoryManager Instance { get; private set; }

    public event Action OnInventoryChanged;
    public event Action<QuestInventoryItem> OnItemAdded;
    public event Action<int> OnItemRemoved;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void AddItem(QuestInventoryItem item)
    {
        _inventory.Add(item);
        //Debug.Log($"Item {item.itemId} just pick uped!");
        OnInventoryChanged?.Invoke();
        OnItemAdded?.Invoke(item);
    }

    public bool DoesHaveItem(QuestInventoryItem item)
    {
        return _inventory.Contains(item);
    }
    public bool DoesHaveItem(string itemId)
    {
        foreach (QuestInventoryItem item in _inventory)
        {
            if(item.itemId == itemId) return true;
        }
        return false;
    }

    public bool DoesHaveAnyItem()
    {
        return _inventory.Count > 0;
    }

    public QuestInventoryItem GetItem(int index)
    {
        return _inventory[index];
    }

    public void RemoveItem(int index)
    {
        if (index < 0 || index >= _inventory.Count) return;
        _inventory.RemoveAt(index);

        OnInventoryChanged?.Invoke();
        OnItemRemoved?.Invoke(index);
    }

    public void RemoveItem(QuestInventoryItem item)
    {
        if (DoesHaveItem(item))
        {
            int itemIndex = _inventory.IndexOf(item);
            _inventory.Remove(item);

            OnInventoryChanged?.Invoke();
            OnItemRemoved?.Invoke(itemIndex);
        }
    }

    public void SelectItem(int id)
    {
        SelectedItemId = id;
    }
}
