using System;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

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
    [SerializeField] private bool _isRotateClockwise = true;
    [SerializeField] private bool StartSignal = true;
    [SerializeField] private float _rotateDelay = 0.4f;
    private float _rotateTimer = 0f;
    private bool _isPossibeToRotate = false;
    private bool _isFailedToLoadConfig = false;
    private bool _isInited = false;

    private bool _isOneShoot = false;
    private bool _isAvailableToExecuteOnEndAction = true;
    private ISignalAction _actionOnEnd;

    public event Action<SignalsManager, SignalLine, bool> OnSignalStateChanged;
    public event Action<SignalsManager, SignalLine, bool> OnLineRotated;
    public event Action OnPuzzleSolved;

    public void Init()
    {
        if (_isInited) return;

        _actionOnEnd = GetComponent<ISignalAction>();
        if (_actionOnEnd == null) _isAvailableToExecuteOnEndAction = false;
        InitConfig();
        _isInited = true;
    }

    private void InitConfig()
    {
        if (_linesConfig == null || _linesConfig.signals == null || _linesConfig.signals.Length == 0)
        {
            Debug.LogError("Чё-то не так с конфигом");
            _isFailedToLoadConfig = true;
            return;
        }

        SignalLineConfig[] signalConfigs = _linesConfig.signals;
        _isOneShoot = _linesConfig.IsOneShoot;
        _signals = new SignalLine[signalConfigs.Length];
        _startDirection = _linesConfig.startDirection;

        SignalLine lastSignal = null;
        for (int i = _signals.Length - 1; i >= 0; i--)
        {
            _signals[i] = new SignalLine(i, signalConfigs[i].signal, signalConfigs[i].direction, lastSignal);
            lastSignal = _signals[i];
        }
    }

    private void Update()
    {
        if (!_isPossibeToRotate)
        {
            _rotateTimer += Time.deltaTime;
            if (_rotateTimer >= _rotateDelay) _isPossibeToRotate = true;
        }
    }

    private void OnEnable()
    {
        if (!_isInited) Init();
        if (_isFailedToLoadConfig) return;
        ForceToUpdateActiveSignals();
    }

    public SignalLine GetSignal(int id)
    {
        if(id < 0 || id >= _signals.Length) return null;
        return _signals[id];
    }

    private void ForceToUpdateActiveSignals()
    {
        foreach (var signal in _signals)
        {
            if(signal.IsLineActive) OnSignalStateChanged?.Invoke(this, signal, signal.IsLineActive);
        }
    }

    public void RecalculateSignals()
    {
        // Делаем перерасчёт активности сигналов при изменении

        // Если не получилось загрузить конфиг - выходим
        if(_isFailedToLoadConfig) return;
        // Ровно как если нет сигнала изначально
        if(!StartSignal) return;

        bool isFail = !StartSignal;
        SignalDirection direction = _startDirection;


        foreach (SignalLine signal in _signals)
        {
            if (isFail)
            {
                Try2ChangeSignalLineState(signal, false);
                continue;
            }

            // Проверяем сигнал, лежащий на противоположной стороне
            // логично, что если сигнал идёт наверх, то надо проверить нижний вход
            // Если сигнал идёт направо - то проверить левый

            if (signal.GetSpecificSignal((int)direction + 2))
            {
                //Debug.Log($"This signal is positive due to direction is {direction.ToString()} and opposite from it direction is {(int)direction + 2}");
                Try2ChangeSignalLineState(signal, true);
                direction = signal.CurrentDirection;

                // Если мы можем попать на этот провод, но дальше нет, то это тоже надо обработать
                if (!signal.GetSpecificSignal(direction))
                {
                    isFail = true;
                }
            }
            else
            {
                Try2ChangeSignalLineState(signal, false);
                isFail = true;
            }

            // Конец цепи
            if (signal.NextSignalLine == null) break;
        }

        if (isFail)
        {
            if (!_isAvailableToExecuteOnEndAction) return;
            // Если провалено, и это тот сценарий, когда головоломка была решённой а стала не решённой
            // то отменяем решение
            if (_actionOnEnd.IsExecuted) _actionOnEnd.Undo();
            return;
        }

        // Если всё прошло хорошо - обрабатываем что-то
        OnPuzzleSolved?.Invoke();

        if (_isAvailableToExecuteOnEndAction) ExecuteOnEndAction();
    }

    private void ExecuteOnEndAction()
    {
        _actionOnEnd.Execute();
        if (_isOneShoot) _isAvailableToExecuteOnEndAction = false;
    }

    public void RotateLine(int id)
    {
        if(_isFailedToLoadConfig) return;
        if (!_isPossibeToRotate) return;
        if (id < 0 || id >= _signals.Length) return;

        SignalLine signal = _signals[id];

        //Debug.Log($"RotateLine: Trying to rotate this\n{signal}");
        signal.Rotate(_isRotateClockwise);
        //Debug.Log($"RotateLine: Just rotated this\n{signal}");
        OnLineRotated?.Invoke(this, signal, _isRotateClockwise);

        RecalculateSignals();
        Debug.Log($"RotateLine: After recalculation:\n{signal}");

        _isPossibeToRotate = false;
        _rotateTimer = 0f;
    }

    private void Try2ChangeSignalLineState(SignalLine signalLine, bool newState)
    {
        //Debug.Log($"Trying to change signal line state {signalLine.ID} from {signalLine.IsLineActive} to {newState}");
        if (signalLine.IsLineActive == newState) return;

        signalLine.IsLineActive = newState;
        OnSignalStateChanged?.Invoke(this, signalLine, newState);
    }
}
