using System.Collections;
using TMPro;
using UnityEngine;

public class ClickerUIMoney : MonoBehaviour
{
    private ClickerManager _clickerManager;
    [SerializeField] private TextMeshProUGUI[] _moneyText;
    [SerializeField] private TextMeshProUGUI _incomePerTickText;

    [Header("Animation Settings")]
    [SerializeField] private float _animationDuration = 0.5f;

    private float _currentDisplayedMoney;
    private float _targetMoney;
    private Coroutine _animationCoroutine;

    private void Start()
    {
        _clickerManager = ClickerManager.Instance;
        _clickerManager.OnMoneyChanged += OnMoneyChanged;

        // Инициализация начального значения
        _currentDisplayedMoney = _clickerManager.Money;
        _targetMoney = _currentDisplayedMoney;
        UpdateText(_currentDisplayedMoney);
    }

    private void OnDestroy()
    {
        if (_clickerManager != null)
        {
            _clickerManager.OnMoneyChanged -= OnMoneyChanged;
        }
    }

    private void OnMoneyChanged(float newMoney)
    {
        _targetMoney = newMoney;

        // Останавливаем предыдущую анимацию, если она была
        if (_animationCoroutine != null)
        {
            StopCoroutine(_animationCoroutine);
        }

        if (Mathf.Abs(newMoney - _currentDisplayedMoney) <= 1f)
        {
            _currentDisplayedMoney = newMoney;
            UpdateText(_currentDisplayedMoney);
            return;
        }

        _animationCoroutine = StartCoroutine(AnimateMoney());
    }

    private IEnumerator AnimateMoney()
    {
        float elapsed = 0f;
        float startMoney = _currentDisplayedMoney;

        while (elapsed < _animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / _animationDuration;

            // Плавная интерполяция (ease-out)
            t = 1f - Mathf.Pow(1f - t, 3f);

            _currentDisplayedMoney = Mathf.Lerp(startMoney, _targetMoney, t);
            UpdateText(_currentDisplayedMoney);

            yield return null;
        }

        // Гарантируем точное конечное значение
        _currentDisplayedMoney = _targetMoney;
        UpdateText(_currentDisplayedMoney);
    }

    private void UpdateText(float money)
    {
        foreach(var text in _moneyText)
            text.text = NumsFormatter.FormatMoney(money);
        _incomePerTickText.text = $"{_clickerManager.IncomePerTick}/c";
    }
}