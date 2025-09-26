using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IConditionToHit
{
    // ��������� �������, ��� ������� ������� ����� ���� ������ ����
    // SRP - �����, ���������������� ���������, ��� �������, ���� �� ���������
    // �� ������ ��������� ���� ������

    abstract bool IsPossibleToHit();
    // ��� ������������� ����� ��������� �������, � �������, ���� �� � ������� ���
    // ��� ���� �� ���������� ����
    // ��� ����� ������ �������
}
