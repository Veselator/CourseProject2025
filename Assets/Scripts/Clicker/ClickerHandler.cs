using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

public class ClickerHandler : MonoBehaviour
{
    // ��������� ������-������ ClickerManager

    private ClickerManager _clickerManager;
    [SerializeField] private BoosterHandler[] _boosterHandlers;

    // ����� ������ ����
    // �� ���� ��� ����� �������� ����� �� ���� ��������
    private const float timePerTick = 1f;
    private float timer = 0f;

    public static ClickerHandler Instance { get; private set; }

    public event Action<Vector2, float> OnUserClicked;
    private void Awake()
    {
        Init(new ClickerManager());
    }

    private void Start()
    {
        UpdateBoostersVisibility();
    }

    private void Update()
    {
        UpdateTimer();
        CheckTick();
    }

    private void Init(ClickerManager clickerManager)
    {
        _clickerManager = clickerManager;
        if (Instance == null) Instance = this;

        InitBoosters();
    }

    private void InitBoosters()
    {
        for (int i = 0; i < _boosterHandlers.Length; i++)
        {
            // �������� �� ������� ������� �������
            // �����, ���-�� ����������� ����� �������
            _boosterHandlers[i].OnBoosterBought += UpdateBoostersVisibility;

            // ���� �� ������ ������ - �� ��������� ���
            if (i > 0) _boosterHandlers[i].gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < _boosterHandlers.Length; i++)
        {
            _boosterHandlers[i].OnBoosterBought -= UpdateBoostersVisibility;
        }
    }

    private void UpdateTimer()
    {
        timer += Time.deltaTime;
    }

    private void CheckTick()
    {
        if (timer > timePerTick)
        {
            timer = 0f;
            ApplyBoosters();
        }
    }

    private void ApplyBoosters()
    {
        if (_boosterHandlers.Length == 0) return;
        float totalSum = 0f;

        foreach (var boosterHandler in _boosterHandlers)
        {
            // ��� ������� � ��� ����������� �� ������
            // ���� ���-�� ���� �� ������ - ������, � ��� ������� ������ �� �������
            // ��� ������ ������ ���������
            if (!boosterHandler.IsBought) break;
            if (boosterHandler.CurrentNumOfUpgrades == 0) continue;

            totalSum += boosterHandler.CurrentIncomePerTick;
            //_clickerManager.ChangeMoney(boosterHandler.CurrentIncomePerTick);
        }

        Debug.Log($"Total sum is {totalSum}, current money {_clickerManager.Money}");
        _clickerManager.IncomePerTick = totalSum;
        _clickerManager.ChangeMoney(totalSum);
    }

    public void ProcessUserClick()
    {
        Debug.Log("User clicked!");
        _clickerManager.ChangeMoney(_clickerManager.IncomePerClick);

        // �������� ������� ����� ����
        Vector3 mouseScreenPosition = Input.mousePosition;
        Vector3 cursorWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        OnUserClicked?.Invoke(cursorWorldPosition, _clickerManager.IncomePerClick);
    }

    private void UpdateBoostersVisibility()
    {
        // ����� ��������� ��� ����, ���-�� ����������
        // ������, ������� �� ����� �������
        // ��� ������� ����� �������� ����� ��������

        int lastBoughtIndex = -1;
        for (int i = 0; i < _boosterHandlers.Length; i++)
        {
            if (_boosterHandlers[i].IsBought)
            {
                lastBoughtIndex = i;
                continue;
            }

            if(i == lastBoughtIndex + 1)
            {
                // �� ����� ��������� ��� ��������
                _boosterHandlers[i].ShowAnimation();
                return;
            }
        }
    }
}
