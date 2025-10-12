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
        // ������ UI ������� � ���������
        GameObject newItem = Instantiate(_itemPrefab, InventoryContentParent);
        newItem.GetComponent<QuestInventoryItemHandler>().Init(_inventoryManager, item, _items.Count);
        _items.Add(newItem);

        // ��������� �������� �������
        StartPickupAnimation(item, newItem);
    }

    private void StartPickupAnimation(QuestInventoryItem item, GameObject inventoryUIItem)
    {
        // �������� ������� ������� �������� (������ �� ��� ������)
        Vector3 worldPosition = item.worldPickupPosition; // ����� �������� ��� ����!

        // ������ ������������� �����
        GameObject animatedItem = Instantiate(animatedItemPrefab, canvas.transform);

        // ����������� � ��������� ��������
        UINewItemAnimation animation = animatedItem.GetComponent<UINewItemAnimation>();
        if (animation != null)
        {
            animation.Init(worldPosition, inventoryUIItem, canvas, item.itemIcon);
        }
        else
        {
            Debug.LogError("UINewItemAnimation component not found on animatedItemPrefab!");
            Destroy(animatedItem);
            inventoryUIItem.SetActive(true); // ����� ���������� ��� ��������
        }
    }

    private void RemoveItem(int id)
    {
        if (id >= 0 && id < _items.Count)
        {
            Destroy(_items[id]);
            _items.RemoveAt(id);

            // ��������� id 
            for (int i = id; i < _items.Count; i++)
            {
                _items[i].GetComponent<QuestInventoryItemHandler>().id = i;
            }
        }
    }
}