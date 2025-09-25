using UnityEngine;

[System.Serializable]
public struct Damage
{
    [Min(0f)]
    public float damageHealth; // ������� ������ ����� ��� ����� ��� ���� ����� �����
    [Min(0f)]
    public float damageArmor; // ������� ������ ����� � �����
    [Min(0f)]
    public float damageMultiplier;
}
