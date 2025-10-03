using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "YetAnotherClickerBooster", menuName = "Clicker/Booster")]
public class ClickerBooster : ScriptableObject
{
    // ����� �� ����� �������
    public float incomePerUnit;
    // ���� ��� �������������
    public float priceToUnlock;
    // ������� ���� �� ������� ��������
    public float basePriceForUnit;
    // ��������� ���� �� ������ �������
    public float priceScalerFactor;
}
