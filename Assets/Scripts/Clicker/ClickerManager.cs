using System;
using UnityEngine;

public class ClickerManager
{
    // �����, ���������� ��������������� �� �������� ���������� � �������
    private float _money = 0f;
    private float _cachedIncomePerClick = -1f; // ��� ��� ��������
    private bool _incomePerClickDirty = true;

    public float Money
    {
        get => _money;
        set
        {
            // �����������: �������� ��������� ����� ������� �������
            float newValue = Mathf.Max(value, 0f);
            if (Mathf.Approximately(newValue, _money)) return;

            _money = newValue;
            OnMoneyChanged?.Invoke(_money);
        }
    }

    // �������� ��� ������ - �� �������� ��� ���� �������������
    private float _priceFactor = 1.25f;
    public float PriceFactor
    {
        get => _priceFactor;
        private set
        {
            if (Mathf.Approximately(value, _priceFactor)) return;
            _priceFactor = value;
            OnPriceFactorChanged?.Invoke(_priceFactor);
        }
    }

    public static ClickerManager Instance { get; private set; }
    public float IncomePerTick { get; set; }

    // ����������, ������� �������� ������� ����� ������
    private float _baseIncomePerClick = 1f;
    public float BaseIncomePerClick
    {
        get => _baseIncomePerClick;
        set
        {
            _baseIncomePerClick = value;
            _incomePerClickDirty = true; // �������� ��� ����� ����������
        }
    }

    private float _incomePerClickMultiplier = 1.0f;
    public float IncomePerClickMultiplier
    {
        get => _incomePerClickMultiplier;
        set
        {
            _incomePerClickMultiplier = value;
            _incomePerClickDirty = true; // �������� ��� ����� ����������
        }
    }

    // ������� ���������� � ������������
    public float IncomePerClick
    {
        get
        {
            if (_incomePerClickDirty)
            {
                _cachedIncomePerClick = _baseIncomePerClick * _incomePerClickMultiplier;
                _incomePerClickDirty = false;
            }
            return _cachedIncomePerClick;
        }
    }

    public event Action<float> OnMoneyChanged;
    public event Action<float> OnPriceFactorChanged;

    public ClickerManager()
    {
        if (Instance == null) Instance = this;
    }

    // �����������: ��������� ����� ��� ��������� ��� �������
    public void ChangeMoneySilent(float deltaMoney)
    {
        _money = Mathf.Max(_money + deltaMoney, 0f);
    }

    public void ChangeMoney(float deltaMoney)
    {
        Money += deltaMoney; // ���������� �������� ��� ������ �������
    }

    public void ModifyPriceFactor(float newPriceFactor)
    {
        PriceFactor = newPriceFactor; // ���������� ��������
    }

    public bool IsAffordable(float price)
    {
        return _money >= price; // ���������� ���� ��������
    }

    public void MultiplyPriceFactor(float multiplier)
    {
        PriceFactor *= multiplier; // ������� ��������� ������������� ����� ��������
    }
}