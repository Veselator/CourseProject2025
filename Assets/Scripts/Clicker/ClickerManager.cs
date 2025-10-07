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
    public float PriceFactor { get; private set; } = 1.25f;
    public static ClickerManager Instance { get; private set; }
    public float IncomePerTick { get; set; }

    // ����������, ������� �������� ������� ����� ������
    private float baseIncomePerClick = 1000f;
    public float BaseIncomePerClick
    {
        get => baseIncomePerClick;
        set => baseIncomePerClick = value;
    }

    public float IncomePerClick => baseIncomePerClick * IncomePerClickMultiplier;
    public float IncomePerClickMultiplier = 1.0f;

    public Action<float> OnMoneyChanged;
    public Action<float> OnPriceFactorChanged;

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

    public void MultiplyPriceFactor(float multiplier)
    {
        PriceFactor *= multiplier;
        OnPriceFactorChanged?.Invoke(PriceFactor);
    }
}
