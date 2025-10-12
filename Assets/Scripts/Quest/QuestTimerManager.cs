using System;
using System.Linq;
using UnityEngine;

public class QuestTimerManager : MonoBehaviour
{
    // Мененджер таймера

    // Время, за сколько пройдёт секунда на таймере
    private const float TIME_PER_TICK = 1.1f;
    private const float TIME_PER_TICK_MODIFIER = 1.8f;
    private const float modifyAfterWhatTime = 6f;
    private float _timer = 0f;
    public float Timer => _timer;

    private QuestGameManager _gameManager;

    private float _nextTickTime;
    private int currentLevel = 0;
    [SerializeField] private float[] timersPerLevel;
    public bool IsTimerStopped = false;

    public static QuestTimerManager Instance {  get; private set; }

    public event Action<float> OnTimerChanged;
    public event Action OnTimerExpired;
    public event Action OnTimerReset;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        _nextTickTime = Time.time + TIME_PER_TICK;
        _timer = timersPerLevel[currentLevel];
    }

    private void Start()
    {
        _gameManager = QuestGameManager.Instance;
        _gameManager.OnLevelLoaded += LoadTimer;
    }

    private void OnDestroy()
    {
        _gameManager.OnLevelLoaded -= LoadTimer;
    }

    private void Update()
    {
        if (GlobalFlags.GetFlag(Flags.GameOver) || IsTimerStopped) return;

        if (Time.time >= _nextTickTime)
        {
            _timer -= 1f;
            OnTimerChanged?.Invoke(_timer);
            if (_timer <= modifyAfterWhatTime) _nextTickTime = Time.time + TIME_PER_TICK * TIME_PER_TICK_MODIFIER;
            else _nextTickTime = Time.time + TIME_PER_TICK;

            // Усё, допрыгались
            if(_timer <= 0)
            {
                GlobalFlags.SetFlag(Flags.GameOver);
                OnTimerExpired?.Invoke();
            }
        }
    }

    public void LoadTimer(int levelId)
    {
        if (levelId < 0 || levelId >= timersPerLevel.Count())
        {
            Debug.LogError($"Ты мне какой levelid ({levelId}) передал?");
        }
        currentLevel = levelId;
        _timer = timersPerLevel[currentLevel];
        OnTimerReset?.Invoke();
    }
}
