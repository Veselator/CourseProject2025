using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShootemUpMoneyUI : MonoBehaviour
{
    private ShootemUpScoreManager _scoreManager;
    [SerializeField] private TextMeshProUGUI _moneyText;

    private void Start()
    {
        _scoreManager = ShootemUpScoreManager.Instance;
        _scoreManager.OnMoneyChanged += UpdateText;

        UpdateText();
    }

    private void OnDestroy()
    {
        _scoreManager.OnMoneyChanged -= UpdateText;
    }

    private void UpdateText()
    {
        _moneyText.text = _scoreManager.Score.ToString("D3");
    }
}
