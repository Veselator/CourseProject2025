using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockSecondWeapons : IUpgrade
{
    public string MainText => "³������";
    public string SecondText => "����� �����";
    public void ApplyUpgrade()
    {
        PlayerInstances.Instance.additionalGuns.SetActive(true);
        // ��������, ����������� ���-�� ���������� ��� ��������
        // ���������� ������� ������
    }
}
