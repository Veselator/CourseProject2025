using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestInventoryManager : MonoBehaviour
{
    private List<QuestInventoryItem> _inventory = new List<QuestInventoryItem>();
    public List<QuestInventoryItem> Inventory => _inventory;

    public int SelectedItemId { get; private set; } = -1;
    public QuestInventoryItem SelectedItem => SelectedItemId == -1 || SelectedItemId >= _inventory.Count ? null : _inventory[SelectedItemId];
    public bool IsSelectedAnyItem => SelectedItemId != -1;

    public static QuestInventoryManager Instance { get; private set; }

    public event Action OnInventoryChanged;
    public event Action<QuestInventoryItem> OnItemAdded;
    public event Action<int> OnItemRemoved;
    public event Action<int> OnItemSelected;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void AddItem(QuestInventoryItem item)
    {
        _inventory.Add(item);
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
            if (item.itemId == itemId) return true;
        }
        return false;
    }

    public QuestInventoryItem DoesHaveItemAndIfYesReturnIt(string itemId)
    {
        foreach (QuestInventoryItem item in _inventory)
        {
            if (item.itemId == itemId) return item;
        }
        return null;
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

        // якщо видал€Їмо вибраний елемент - скидаЇмо виб≥р
        if (SelectedItemId == index)
        {
            DeselectItem();
        }
        else if (SelectedItemId > index)
        {
            //  оригуЇмо ≥ндекс, €кщо видал€Їмо елемент перед вибраним
            SelectedItemId--;
        }

        _inventory.RemoveAt(index);
        OnInventoryChanged?.Invoke();
        OnItemRemoved?.Invoke(index);
    }

    public void RemoveItem(QuestInventoryItem item)
    {
        if (DoesHaveItem(item))
        {
            int itemIndex = _inventory.IndexOf(item);
            RemoveItem(itemIndex);
        }
    }

    public void RemoveItem(string itemId)
    {
        QuestInventoryItem tempItem = DoesHaveItemAndIfYesReturnIt(itemId);
        if (tempItem != null)
        {
            RemoveItem(tempItem);
        }
    }

    public void SelectItem(int id)
    {
        if (id < 0 || id >= _inventory.Count)
        {
            DeselectItem();
            return;
        }

        // якщо кл≥каЇмо на той самий елемент - зн≥маЇмо вид≥ленн€
        if (SelectedItemId == id)
        {
            DeselectItem();
        }
        else
        {
            SelectedItemId = id;
            Debug.Log($"Item selected {id}");
            OnItemSelected?.Invoke(id);
        }
    }

    public void DeselectItem()
    {
        SelectedItemId = -1;
        OnItemSelected?.Invoke(-1);
        Debug.Log("Item deselected");
    }
}