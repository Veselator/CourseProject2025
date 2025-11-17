using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrentArmorUITracker : MonoBehaviour
{
    [SerializeField] private ClickerShopManager _shopManager;

    [SerializeField] private Image _linkedArmorTracker;
    [SerializeField] private TMP_Text[] _linkedArmorText;
    private int _currentArmorId = 0;
    private const int _maxArmor = 3;

    private void Start()
    {
        UpdateUI();
        _shopManager.OnArmorPurched += UpdateCurrentArmorUI;
    }

    private void OnDestroy()
    {
        _shopManager.OnArmorPurched -= UpdateCurrentArmorUI;
    }

    private void UpdateCurrentArmorUI(int armorId)
    {
        _currentArmorId = armorId;
        UpdateUI();
    }

    private void UpdateUI()
    {
        _linkedArmorTracker.fillAmount = (float)_currentArmorId / _maxArmor;

        foreach (var text in _linkedArmorText)
        {
            text.text = $"Броня: {_currentArmorId} / {_maxArmor}";
        }
    }
}
