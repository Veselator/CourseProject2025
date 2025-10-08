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
    private const float TIME_PER_TICK = 1.1f;
    private float _timer = 0f;
    private float _nextTickTime; // �����������: ������ �������� ������ ����

    // ��� ��� �����������
    private int _lastActiveBoughtBoosterIndex = -1;
    private float _cachedTotalIncome = 0f;
    private bool _incomeNeedsRecalculation = true;

    // �����������: �������� ������
    private Camera _mainCamera;

    public static ClickerHandler Instance { get; private set; }
    public event Action<Vector2, float> OnUserClicked;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Init(new ClickerManager());
        }
        else
        {
            Destroy(gameObject);
        }

        // �������� ������ ���� ���
        _mainCamera = Camera.main;
    }

    private void Start()
    {
        _nextTickTime = Time.time + TIME_PER_TICK;
        UpdateBoostersVisibility();
    }

    private void Update()
    {
        // �����������: ��������� ����� ������ ����� �����
        if (Time.time >= _nextTickTime)
        {
            ApplyBoosters();
            _nextTickTime = Time.time + TIME_PER_TICK;
        }
    }

    private void Init(ClickerManager clickerManager)
    {
        _clickerManager = clickerManager;
        InitBoosters();
    }

    private void InitBoosters()
    {
        // �����������: ���� ������ �� �������
        for (int i = 0; i < _boosterHandlers.Length; i++)
        {
            var booster = _boosterHandlers[i];

            // �������� �� �������
            booster.OnBoosterBought += OnBoosterBought;
            booster.OnBoosterUpgraded += OnBoosterUpgraded;

            // ���� �� ������ ������ - ���������
            if (i > 0)
            {
                booster.gameObject.SetActive(false);
            }
        }
    }

    private void OnDestroy()
    {
        // �����������: �������� �� null
        if (_boosterHandlers == null) return;

        for (int i = 0; i < _boosterHandlers.Length; i++)
        {
            if (_boosterHandlers[i] != null)
            {
                _boosterHandlers[i].OnBoosterBought -= OnBoosterBought;
                _boosterHandlers[i].OnBoosterUpgraded -= OnBoosterUpgraded;
            }
        }
    }

    // ����� ������ ��� �������
    private void OnBoosterBought()
    {
        _incomeNeedsRecalculation = true;
        RecalculateTotalIncome();
        UpdateBoostersVisibility();
    }

    private void OnBoosterUpgraded()
    {
        _incomeNeedsRecalculation = true;
    }

    private void ApplyBoosters()
    {
        if (_boosterHandlers == null || _boosterHandlers.Length == 0) return;

        // ���������� ��� ���� ����� �� ���������
        if (!_incomeNeedsRecalculation)
        {
            if (_cachedTotalIncome > 0)
            {
                _clickerManager.ChangeMoney(_cachedTotalIncome);
            }
            return;
        }

        // ������������� �����
        RecalculateTotalIncome();

        if (_cachedTotalIncome > 0)
        {
            _clickerManager.IncomePerTick = _cachedTotalIncome;
            _clickerManager.ChangeMoney(_cachedTotalIncome);
        }

        _incomeNeedsRecalculation = false;
    }

    private void RecalculateTotalIncome()
    {
        float totalSum = 0f;
        int lastBoughtIndex = -1; // ��������� ���������� ��� ������������

        // �����������: ���������� ������������ ������ ��� ���������
        int endIndex = _lastActiveBoughtBoosterIndex >= 0
            ? Mathf.Min(_lastActiveBoughtBoosterIndex + 2, _boosterHandlers.Length) // +2 ����� ��������� ���������
            : _boosterHandlers.Length;

        for (int i = 0; i < endIndex; i++)
        {
            var booster = _boosterHandlers[i];

            if (!booster.IsBought)
            {
                // ����� ������ ����������� - �������
                break;
            }

            // ������ ������ - ��������� ������
            lastBoughtIndex = i;

            if (booster.CurrentNumOfUpgrades > 0)
            {
                totalSum += booster.CurrentIncomePerTick;
            }
        }

        // ��������: ��������� ��� ����� �����
        _lastActiveBoughtBoosterIndex = lastBoughtIndex;
        _cachedTotalIncome = totalSum;

        Debug.Log($"Total income: {totalSum}, Money: {_clickerManager.Money}, LastBoughtIndex: {lastBoughtIndex}");
    }

    public void ProcessUserClick()
    {

        float clickIncome = _clickerManager.IncomePerClick;
        _clickerManager.ChangeMoney(clickIncome);

        // �����������: ���������� ������������ ������
        if (_mainCamera != null)
        {
            Vector3 mouseScreenPosition = Input.mousePosition;
            Vector3 cursorWorldPosition = _mainCamera.ScreenToWorldPoint(mouseScreenPosition);
            OnUserClicked?.Invoke(cursorWorldPosition, clickIncome);
        }
    }

    private void UpdateBoostersVisibility()
    {
        int nextBoosterIndex = _lastActiveBoughtBoosterIndex + 1;

        Debug.Log($"UpdateBoostersVisibility: lastBought={_lastActiveBoughtBoosterIndex}, next={nextBoosterIndex}");

        // ��������� ��� ������ �������
        if (nextBoosterIndex >= 0 && nextBoosterIndex < _boosterHandlers.Length)
        {
            if (!_boosterHandlers[nextBoosterIndex].IsBought)
            {
                _boosterHandlers[nextBoosterIndex].ShowAnimation();
            }
        }
    }

    // ����� ��� �������������� ��������� (���� �����)
    public void ForceRecalculateIncome()
    {
        _incomeNeedsRecalculation = true;
        _lastActiveBoughtBoosterIndex = -1;
        RecalculateTotalIncome();
    }
}