using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBooster : MonoBehaviour
{
    // Скрипт для UI отображения бустера

    // Важные элементы интерфейса
    [SerializeField] private GameObject lockBoosterObject;
    [SerializeField] private GameObject mainGroup;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI incomePerSecond;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI priceUnlockText;
    [SerializeField] private Image buyUpgradeButtonImage;
    [SerializeField] private Image buyButtonImage;
    private bool currentButtonState = true;

    private Color buttonNotAvailableColor = Color.gray;
    private Color buttonAvailableColor = Color.white;

    // Текущий бустер
    private BoosterHandler _currentBooster;
    private ClickerManager _clickerManager;

    private void Start()
    {
        _currentBooster = GetComponent<BoosterHandler>();
        _clickerManager = ClickerManager.Instance;

        _clickerManager.OnMoneyChanged += UpdateButtonState;
        InitUIComponents();
    }

    private void OnDestroy()
    {
        _clickerManager.OnMoneyChanged -= UpdateButtonState;
    }

    private void InitUIComponents()
    {
        title.text = _currentBooster.Title;
        levelText.text = $"Lv. {_currentBooster.CurrentNumOfUpgrades}";
        incomePerSecond.text = $"{_currentBooster.CurrentIncomePerTick}/c";
        priceUnlockText.text = $"{_currentBooster.PriceToUnlock}";
        priceText.text = $"{Math.Ceiling(_currentBooster.PriceToUpgrade)}";

        lockBoosterObject.SetActive(true);
        mainGroup.SetActive(false);

        UpdateButtonState(0f);
    }

    // Надеюсь получиться доделать эту курсовую и не совершить суицид
    // 03.10.25
    // Если я останусь живым - то должен оставить здесь комментарий "Всё нормально"

    //

    private void UpdateTextInfo()
    {
        levelText.text = $"Lv. {_currentBooster.CurrentNumOfUpgrades}";
        incomePerSecond.text = $"{_currentBooster.CurrentIncomePerTick}/c";
        priceText.text = $"{Math.Ceiling(_currentBooster.PriceToUpgrade)}";
    }

    private void UpdateButtonState(float _ = 0f)
    {
        if (_currentBooster.IsBought) UpdateUpgradeButtonState(_currentBooster.IsAvailableToUpgrade);
        else UpdateBuyButtonState(_currentBooster.IsAvailableToBuy);
    }

    private void UpdateBuyButtonState(bool isAvailableToBuy)
    {
        if (isAvailableToBuy && !currentButtonState)
        {
            currentButtonState = true;
            buyButtonImage.color = buttonAvailableColor;
        }
        else if (!isAvailableToBuy && currentButtonState)
        {
            currentButtonState = false;
            buyButtonImage.color = buttonNotAvailableColor;
        }
    }

    private void UpdateUpgradeButtonState(bool isAvailableToDoUpgrade)
    {
        if (isAvailableToDoUpgrade && !currentButtonState)
        {
            currentButtonState = true;
            buyUpgradeButtonImage.color = buttonAvailableColor;
        }
        else if (!isAvailableToDoUpgrade && currentButtonState)
        {
            currentButtonState = false;
            buyUpgradeButtonImage.color = buttonNotAvailableColor;
        }
    }

    public void TryToUnlockBooster()
    {
        // _currentBooster.TryToBuy
        // Если уже куплен - то возвращаемся
        if (_currentBooster.IsBought) return;
        // Интерфейс взаимодействия реализован через булевый TryToBuy()
        // Просто и надёжно
        if (!_currentBooster.TryToBuy()) return;

        lockBoosterObject.SetActive(false);
        mainGroup.SetActive(true);

        currentButtonState = true;
        UpdateButtonState();
    }

    public void TryToUpgradeBooster()
    {
        if (_currentBooster.TryToUpgrade())
        {
            // Графически отобразить апгрейд
            UpdateTextInfo();
        }
    }
}
