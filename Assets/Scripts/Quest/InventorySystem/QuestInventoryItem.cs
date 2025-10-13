using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Quest/Item")]
public class QuestInventoryItem : ScriptableObject
{
    [Header("Visual")]
    public Sprite itemIcon;

    [Header("Identification")]
    public string itemId;

    [Header("Usage")]
    public string[] targetItemId; // �� ����� ������� ����� ������������
    public QuestAction actionOnTarget; // ��������, ������� �����������, ���� ������� ������������ �� ����
    public bool IsOneShoot = true;

    [HideInInspector] public Vector3 worldPickupPosition;
}
