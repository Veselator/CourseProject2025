using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestInventoryItemHandler : MonoBehaviour
{
    public QuestInventoryItem LinkedItem { get; private set; }
    private QuestInventoryManager _questInventoryManager;
    public int id = 0;

    private Image _itemImage;
    private Material _normalMaterial;
    private Material _highlightMaterial;

    public void Init(QuestInventoryManager questInvManager, QuestInventoryItem linkedItem, int id)
    {
        _questInventoryManager = questInvManager;
        LinkedItem = linkedItem;
        _itemImage = GetComponent<Image>();
        _itemImage.sprite = LinkedItem.itemIcon;
        this.id = id;

        // Створюємо копію матеріалу для кожного елемента
        _normalMaterial = new Material(Shader.Find("UI/Default"));
        _highlightMaterial = new Material(Shader.Find("Custom/InventoryGlow"));

        // Встановлюємо параметри шейдера
        _highlightMaterial.SetColor("_GlowColor", Color.white);
        _highlightMaterial.SetFloat("_GlowIntensity", 1.5f);
        _highlightMaterial.SetFloat("_GlowSpeed", 3f);

        _itemImage.material = _normalMaterial;

        // Підписуємось на зміни вибору
        _questInventoryManager.OnItemSelected += OnItemSelected;
    }

    private void OnDestroy()
    {
        if (_questInventoryManager != null)
        {
            _questInventoryManager.OnItemSelected -= OnItemSelected;
        }

        // Очищуємо матеріали
        if (_normalMaterial != null) Destroy(_normalMaterial);
        if (_highlightMaterial != null) Destroy(_highlightMaterial);
    }

    private void OnItemSelected(int selectedId)
    {
        // Якщо цей елемент вибраний - застосовуємо highlight
        if (selectedId == id)
        {
            _itemImage.material = _highlightMaterial;
        }
        else
        {
            _itemImage.material = _normalMaterial;
        }
    }

    public void HandleClick()
    {
        _questInventoryManager.SelectItem(id);
    }
}