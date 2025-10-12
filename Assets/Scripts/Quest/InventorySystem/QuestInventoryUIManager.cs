using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestInventoryUIManager : MonoBehaviour
{
    private QuestInventoryManager _inventoryManager;

    [Header("UI References")]
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private Transform InventoryContentParent;
    [SerializeField] private Canvas canvas;

    [Header("Animation")]
    [SerializeField] private GameObject animatedItemPrefab;
    [SerializeField] private Transform InventoryParent;

    private List<GameObject> _items = new List<GameObject>();

    private void Start()
    {
        _inventoryManager = QuestInventoryManager.Instance;
        _inventoryManager.OnItemAdded += AddItem;
        _inventoryManager.OnItemRemoved += RemoveItem;

        if (canvas == null)
        {
            canvas = GetComponentInParent<Canvas>();
        }
    }

    private void OnDestroy()
    {
        if (_inventoryManager != null)
        {
            _inventoryManager.OnItemAdded -= AddItem;
            _inventoryManager.OnItemRemoved -= RemoveItem;
        }
    }

    private void AddItem(QuestInventoryItem item)
    {
        // Создаём UI предмет в инвентаре
        GameObject newItem = Instantiate(_itemPrefab, InventoryContentParent);
        newItem.GetComponent<QuestInventoryItemHandler>().Init(_inventoryManager, item, _items.Count);
        _items.Add(newItem);

        // Запускаем анимацию подбора
        StartPickupAnimation(item, newItem);
    }

    private void StartPickupAnimation(QuestInventoryItem item, GameObject inventoryUIItem)
    {
        // Получаем мировую позицию предмета (откуда он был поднят)
        Vector3 worldPosition = item.worldPickupPosition; // Нужно добавить это поле!

        // Создаём анимированную копию
        GameObject animatedItem = Instantiate(animatedItemPrefab, canvas.transform);

        // Настраиваем и запускаем анимацию
        UINewItemAnimation animation = animatedItem.GetComponent<UINewItemAnimation>();
        if (animation != null)
        {
            animation.Init(worldPosition, inventoryUIItem, canvas, item.itemIcon);
        }
        else
        {
            Debug.LogError("UINewItemAnimation component not found on animatedItemPrefab!");
            Destroy(animatedItem);
            inventoryUIItem.SetActive(true); // Сразу показываем без анимации
        }
    }

    private void RemoveItem(int id)
    {
        if (id >= 0 && id < _items.Count)
        {
            Destroy(_items[id]);
            _items.RemoveAt(id);

            // Обновляем id 
            for (int i = id; i < _items.Count; i++)
            {
                _items[i].GetComponent<QuestInventoryItemHandler>().id = i;
            }
        }
    }
}