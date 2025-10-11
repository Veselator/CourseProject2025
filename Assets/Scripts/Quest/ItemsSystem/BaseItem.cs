using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseItem : MonoBehaviour, IInteractable
{
    // Базовый класс предмета
    // От него наследуется интерактивный предмет и предмет подбираемый
    private ItemVisual _itemVisual;

    // Нужен для идентификации предмета
    public string itemID;

    private void Start()
    {
        _itemVisual = GetComponent<ItemVisual>();
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

    public abstract bool CanInteract();
    public abstract void Interact();
}
