using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    float MaximumHealth { get; }
    float CurrentHealth { get; set; }
    GameObject Instance { get; }

    // ���� ���� �����, �� ���� ��������
    float MaximumArmor { get; } // ������� �������� ����� ���� �����
    float Armor { get; set; } // ����� ������� ���������� �����
    // �������� ����� �� ����� �������� � struct Damage ��� ������� ��������
    //float ArmorFactor { get; set; } // ����� ������� �� ������������ ����� ������ �� �����

    bool DoesHaveArmor { get; }
    IConditionToHit conditionToHit { get; set; } // ��������� �������, ��� ���������� �������� ����� �������������� ����
    bool IsDied { get; }
    Action OnDamaged { get; set; }
    Action OnDeath { get; set;  }
    abstract void TakeDamage(Damage damage); // �������� damage �� struct damage, ��� ������� ����������� �����
    // Ps OnCollisionEnter2D ������������ ����
}
