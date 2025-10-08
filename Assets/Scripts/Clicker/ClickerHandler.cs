using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

public class ClickerHandler : MonoBehaviour
{
    // Реализует бизнес-логику ClickerManager
    private ClickerManager _clickerManager;

    [SerializeField] private BoosterHandler[] _boosterHandlers;

    // Время одного тика
    private const float TIME_PER_TICK = 1.1f;
    private float _timer = 0f;
    private float _nextTickTime; // Оптимизация: вместо проверки каждый кадр

    // Кеш для оптимизации
    private int _lastActiveBoughtBoosterIndex = -1;
    private float _cachedTotalIncome = 0f;
    private bool _incomeNeedsRecalculation = true;

    // Оптимизация: кешируем камеру
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

        // Кешируем камеру один раз
        _mainCamera = Camera.main;
    }

    private void Start()
    {
        _nextTickTime = Time.time + TIME_PER_TICK;
        UpdateBoostersVisibility();
    }

    private void Update()
    {
        // Оптимизация: проверяем время только когда нужно
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
        // Оптимизация: один проход по массиву
        for (int i = 0; i < _boosterHandlers.Length; i++)
        {
            var booster = _boosterHandlers[i];

            // Подписка на события
            booster.OnBoosterBought += OnBoosterBought;
            booster.OnBoosterUpgraded += OnBoosterUpgraded;

            // Если не первый бустер - выключаем
            if (i > 0)
            {
                booster.gameObject.SetActive(false);
            }
        }
    }

    private void OnDestroy()
    {
        // Оптимизация: проверка на null
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

    // Новые методы для событий
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

        // Используем кеш если доход не изменился
        if (!_incomeNeedsRecalculation)
        {
            if (_cachedTotalIncome > 0)
            {
                _clickerManager.ChangeMoney(_cachedTotalIncome);
            }
            return;
        }

        // Пересчитываем доход
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
        int lastBoughtIndex = -1; // Локальная переменная для отслеживания

        // Оптимизация: используем кешированный индекс как подсказку
        int endIndex = _lastActiveBoughtBoosterIndex >= 0
            ? Mathf.Min(_lastActiveBoughtBoosterIndex + 2, _boosterHandlers.Length) // +2 чтобы проверить следующий
            : _boosterHandlers.Length;

        for (int i = 0; i < endIndex; i++)
        {
            var booster = _boosterHandlers[i];

            if (!booster.IsBought)
            {
                // Нашли первый некупленный - выходим
                break;
            }

            // Бустер куплен - обновляем индекс
            lastBoughtIndex = i;

            if (booster.CurrentNumOfUpgrades > 0)
            {
                totalSum += booster.CurrentIncomePerTick;
            }
        }

        // КРИТИЧНО: обновляем кеш ПОСЛЕ цикла
        _lastActiveBoughtBoosterIndex = lastBoughtIndex;
        _cachedTotalIncome = totalSum;

        Debug.Log($"Total income: {totalSum}, Money: {_clickerManager.Money}, LastBoughtIndex: {lastBoughtIndex}");
    }

    public void ProcessUserClick()
    {

        float clickIncome = _clickerManager.IncomePerClick;
        _clickerManager.ChangeMoney(clickIncome);

        // Оптимизация: используем кешированную камеру
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

        // Проверяем что индекс валиден
        if (nextBoosterIndex >= 0 && nextBoosterIndex < _boosterHandlers.Length)
        {
            if (!_boosterHandlers[nextBoosterIndex].IsBought)
            {
                _boosterHandlers[nextBoosterIndex].ShowAnimation();
            }
        }
    }

    // Метод для форсированного пересчета (если нужно)
    public void ForceRecalculateIncome()
    {
        _incomeNeedsRecalculation = true;
        _lastActiveBoughtBoosterIndex = -1;
        RecalculateTotalIncome();
    }
}