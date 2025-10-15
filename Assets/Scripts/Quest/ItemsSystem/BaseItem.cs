using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseItem : MonoBehaviour, IInteractable, IMessageReceiver
{
    // Базовый класс предмета
    // От него наследуется интерактивный предмет и предмет подбираемый
    private ItemVisual _itemVisual;

    // Нужен для идентификации предмета
    public string itemID;
    [SerializeField] private bool IsNeedToRegisterObject = true;
    [SerializeField] private bool isNeedToHide = false;

    protected virtual void Start()
    {
        _itemVisual = GetComponent<ItemVisual>();
        if(IsNeedToRegisterObject) QuestObjectRegistry.Instance.AddObject(itemID, gameObject);
        if(isNeedToHide) gameObject.SetActive(false);
    }

    protected void HideVisual()
    {
        // Когда объект перестаёт быть интерактивным
        if (_itemVisual != null) _itemVisual.Highlight(false);
    }

    private void OnMouseEnter()
    {
        if(CanInteract() && _itemVisual != null) _itemVisual.Highlight(true);
    }

    private void OnMouseExit()
    {
        if (CanInteract() && _itemVisual != null) _itemVisual.Highlight(false);
    }

    private void OnMouseDown()
    {
        if (CanInteract()) Interact();
    }

    // Для реализации дополнительной логики
    // По типу откручивания вентиляционной крышки
    public virtual void ProcessMessage(string message)
    {
        Debug.Log($"Lol, I got a message {message}");
    }

    public abstract bool CanInteract();
    public abstract void Interact();
}
