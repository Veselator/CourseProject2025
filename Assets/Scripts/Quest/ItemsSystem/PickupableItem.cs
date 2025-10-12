using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupableItem : BaseItem
{
    [Header("�������, �� ������� ���������")]
    [SerializeField] private QuestInventoryItem itemData; // ��, �� ����� ������� ���������
    [Header("��������, ������� ����������� ��� ������� ��������")]
    [SerializeField] private QuestAction additionalActions;

    protected override void Start()
    {
        itemID = itemData.itemId; // �����
        itemData.worldPickupPosition = transform.position;
        base.Start();
    }

    public override bool CanInteract() => true;

    public override void Interact()
    {
        QuestInventoryManager.Instance.AddItem(itemData);
        if (additionalActions != null) QuestActionProccessor.Instance.ProcessAction(additionalActions, this.gameObject);
        Destroy(gameObject);
    }
}
