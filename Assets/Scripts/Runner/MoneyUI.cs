using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoneyUI : MonoBehaviour
{
    private MoneyTracker _moneyTracker;
    [SerializeField] private TextMeshProUGUI _moneyText;

    private void Start()
    {
        _moneyTracker = MoneyTracker.Instance;
        _moneyTracker.OnMoneyChanged += UpdateText;

        UpdateText();
    }

    private void OnDestroy()
    {
        _moneyTracker.OnMoneyChanged -= UpdateText;
    }

    private void UpdateText()
    {
        _moneyText.text = _moneyTracker.Money.ToString();
    }
}
