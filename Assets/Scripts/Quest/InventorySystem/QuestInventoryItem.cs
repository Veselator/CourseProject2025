using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Item", menuName = "Quest/Item")]
public class QuestInventoryItem : ScriptableObject
{
    [Header("Visual")]
    public Sprite itemIcon;

    [Header("Identification")]
    public string itemId;

    [Header("Usage")]
    public string targetItemId; // �� ����� ������� ����� ������������
    public QuestAction actionOnTarget; // ��������, ������� �����������, ���� ������� ������������ �� ����
}
