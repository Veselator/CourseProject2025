using System;
using UnityEngine;

public class SignalsManager : MonoBehaviour
{
    // Отвечает за проверку правильности соединения линий головоломки
    // Важный момент - в рамках текущей арххитектуры для конкретного SignalsManager
    // есть конкретное решение
    // Я имею в виду что CurrentDirection в каждом SignalLine не может меняться
    // Дополнительные блоки, для отвлечения внимания потребуется реализовывать через
    // дополнительный SignalsManager
    // Иначе просто не будут работать

    [SerializeField] private SignalLinesConfig _linesConfig;
    private SignalDirection _startDirection;
    private SignalLine[] _signals;
    [SerializeField] private bool _isRotateCLockwise = true;
    [SerializeField] private bool StartSignal = true;

    public event Action<SignalLine, bool> OnSignalStateChanged;
    public event Action<SignalLine> OnLineRotated;
    private void Awake()
    {
        Init();
        RecalculateSignals();
    }

    private void Init()
    {
        SignalLineConfig[] signalConfigs = _linesConfig.signals;
        _signals = new SignalLine[signalConfigs.Length];

        SignalLine lastSignal = null;
        for (int i = _signals.Length - 1; i >= 0; i--)
        {
            _signals[i] = new SignalLine(i, signalConfigs[i].signal, signalConfigs[i].direction, lastSignal);
            lastSignal = _signals[i];
        }
    }

    private void RecalculateSignals()
    {
        // Делаем перерасчёт активности сигналов при изменеении
        bool isFail = !StartSignal;
        SignalDirection direction = _startDirection;

        foreach (SignalLine signal in _signals)
        {
            if (isFail) Try2ChangeSignalLineState(signal, false);

            // Проверяем сигнал, лежащий на противоположной стороне
            // логично, что если сигнал идёт наверх, то надо проверить нижний вход
            // Если сигнал идёт направо - то проверить левый

            if(signal.GetSpecificSignal((int)direction + 2))
            {
                Try2ChangeSignalLineState(signal, true);
                direction = signal.CurrentDirection;
            }
            else
            {
                Try2ChangeSignalLineState(signal, false);
                isFail = true;
            }

            // Конец цепи
            if (signal.NextSignalLine == null) break;
        }

        if (isFail) return;

        // Если всё прошло хорошо - обрабатываем что-то
    }

    public void RotateLine(int id)
    {
        if (id < 0 || id >= _signals.Length) return;

        SignalLine signal = _signals[id];

        signal.Rotate(_isRotateCLockwise);
        OnLineRotated?.Invoke(signal);

        RecalculateSignals();
    }

    private void Try2ChangeSignalLineState(SignalLine signalLine, bool newState)
    {
        if (signalLine.IsLineActive == newState) return;

        signalLine.IsLineActive = newState;
        OnSignalStateChanged?.Invoke(signalLine, newState);
    }
}
