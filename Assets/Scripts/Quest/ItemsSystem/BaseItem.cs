using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseItem : MonoBehaviour, IInteractable
{
    // ������� ����� ��������
    // �� ���� ����������� ������������� ������� � ������� �����������
    private ItemVisual _itemVisual;

    // ����� ��� ������������� ��������
    public string itemID;
    [SerializeField] private bool IsNeedToRegisterObject = true;

    protected virtual void Start()
    {
        _itemVisual = GetComponent<ItemVisual>();
        if(IsNeedToRegisterObject) QuestObjectRegistry.Instance.AddObject(itemID, gameObject);
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
