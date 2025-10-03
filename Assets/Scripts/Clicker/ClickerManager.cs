using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickerManager
{
    // �����, ���������� ��������������� �� �������� ���������� � �������

    private float money = 0f;
    public float Money {
        get => money;
        set
        {
            money = Mathf.Max(value, 0f);
        }
    }

    // �������� ��� ������ - �� �������� ��� ���� �������������
    public float PriceFactor { get; set; } = 1f;
    public static ClickerManager Instance { get; private set; }

    // ����������, ������� �������� ������� ����� ������
    private float baseIncomePerClick = 1f;
    public float IncomePerClick => baseIncomePerClick * incomePerClickMultiplier;
    public float incomePerClickMultiplier = 1f;

    public Action<float> OnMoneyChanged;

    public ClickerManager()
    {
        if (Instance == null) Instance = this;
    }

    public void ChangeMoney(float deltaMoney)
    {
        Money += deltaMoney;
        OnMoneyChanged?.Invoke(Money);
    }

    public void ModifyPriceFactor(float newPriceFactor)
    {
        PriceFactor = newPriceFactor;
    }

    public bool IsAffordable(float price)
    {
        return Money >= price;
    }
}
