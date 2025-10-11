using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestInventoryUIManager : MonoBehaviour
{
    private QuestInventoryManager _inventoryManager;
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private Transform InventoryContentParent;
    private List<GameObject> _items = new List<GameObject>();

    private void Start()
    {
        _inventoryManager = QuestInventoryManager.Instance;
        _inventoryManager.OnItemAdded += AddItem;
        _inventoryManager.OnItemRemoved += RemoveItem;
    }

    private void OnDestroy()
    {
        _inventoryManager.OnItemAdded -= AddItem;
        _inventoryManager.OnItemRemoved -= RemoveItem;
    }

    private void AddItem(QuestInventoryItem item)
    {
        GameObject newItem = Instantiate(_itemPrefab, InventoryContentParent);
        newItem.GetComponent<QuestInventoryItemHandler>().Init(item, _items.Count);
        _items.Add(newItem);
    }

    private void RemoveItem(int id)
    {
        Destroy(_items[id]);
        _items.RemoveAt(id);
        
        // Обновляем id 
        for (int i = id; i < _items.Count; i++)
        {
            _items[i].GetComponent<QuestInventoryItemHandler>().id = id;
        }
    }
}
