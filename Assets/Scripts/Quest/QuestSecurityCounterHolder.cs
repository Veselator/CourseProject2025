using System;
using TMPro;
using UnityEngine;

public class QuestSecurityCounterHolder : MonoBehaviour
{
    // Отвечает за логику счётчика
    [SerializeField] private HoldingCounter _currentCounter;
    [SerializeField] private TextMeshProUGUI _linkedText;
    private const int MIN_VALUE = 0;
    private const int MAX_VALUE = 9;

    public int CurrentValue { get; private set; } = MIN_VALUE;
    public event Action OnValueChanged;

    private void OnEnable()
    {
        if(_currentCounter.topButton != null) _currentCounter.topButton.OnValueChanged += ProccessChange;
        if(_currentCounter.bottomButton != null) _currentCounter.bottomButton.OnValueChanged += ProccessChange;
    }

    private void OnDisable()
    {
        if (_currentCounter.topButton != null) _currentCounter.topButton.OnValueChanged -= ProccessChange;
        if (_currentCounter.bottomButton != null) _currentCounter.bottomButton.OnValueChanged -= ProccessChange;
    }

    private void ProccessChange(int delta)
    {
        CurrentValue += delta;
        if (CurrentValue > MAX_VALUE) CurrentValue = MIN_VALUE;
        else if (CurrentValue < MIN_VALUE) CurrentValue = MAX_VALUE;
        OnValueChanged?.Invoke();

        UpdateLinkedText();
    }

    private void UpdateLinkedText()
    {
        _linkedText.text = CurrentValue.ToString();
    }
}

[Serializable]
public struct HoldingCounter
{
    public QuestSecurityCounterButton topButton;
    public QuestSecurityCounterButton bottomButton;
}
