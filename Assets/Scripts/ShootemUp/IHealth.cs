using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    float MaximumHealth { get; set; }
    float CurrentHealth { get; set; }
    GameObject Instance { get; }

    // ���� ���� �����, �� ���� ��������
    float MaximumArmor { get; set; } // ������� �������� ����� ���� �����
    float Armor { get; set; } // ����� ������� ���������� �����
    // �������� ����� �� ����� �������� � struct Damage ��� ������� ��������
    //float ArmorFactor { get; set; } // ����� ������� �� ������������ ����� ������ �� �����

    bool DoesHaveArmor { get; }
    IConditionToHit conditionToHit { get; set; } // ��������� �������, ��� ���������� �������� ����� �������������� ����
    bool IsDied { get; }
    float CurrentHealthInPercentage { get; }
    float CurrentArmorInPercentage { get; }
    Action OnDamaged { get; set; }
    Action OnHealthChanged { get; set; }
    Action OnArmoryDestoyed { get; set; }
    Action OnArmorChanged { get; set; }
    Action OnDeath { get; set;  }
    abstract void TakeDamage(Damage damage); // �������� damage �� struct damage, ��� ������� ����������� �����
    // Ps OnCollisionEnter2D ������������ ����
}
