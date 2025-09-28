using System;
using UnityEngine;

public class DistanceTracker : MonoBehaviour
{
    // Distance tracker отслеживает пройденное рассто€ние и скорость игрока
    // –еагирует на глобальные флаги, такие как RUNNER_IS_ROTATING и RUNNER_STAGE_1_PASSED
    // ¬ызывает событи€ при достижении определенных дистанций
    //  ак вариант, разделить на два класса: SpeedTracker и StageTracker
    // SpeedTracker будет отвечать только за скорость и дистанцию
    // StageTracker будет отвечать за этапы и событи€

    public float CurrentSpeed { get; private set; } = 8f;
    private const float START_ACCELERATION = 1f;
    private float currentAcceleration = START_ACCELERATION;
    private float maxAcceleration = 4f;

    public float CurrentDistance { get; private set; }
    private float maxSpeed = 30f;

    public float MaxAcceleration
    {
        get => maxAcceleration;
        set
        {
            if (value <= 0) return;
            maxAcceleration = value;
            currentAcceleration = START_ACCELERATION;
        }
    }

    // «начени€ дистанций дл€ этапов
    // ¬ынесены в константы дл€ удобства настройки
    public readonly float DISTANCE_TO_FIRST_TURN = 700f;
    public readonly float DISTANCE_TO_SECOND_TURN = 1400f;
    public readonly float DISTANCE_TO_END_GAME = 2600f;

    public float ProgressToEnd => CurrentDistance / DISTANCE_TO_END_GAME;

    private float[] turnDistances;

    private PlayerMovementAcrossLevel playerMovementAcrossLevel;
    private PlayerRotationAnimation playerRotationAnimation;
    private SpawnObstacles spawnObstacles;
    [SerializeField] private BaseSpawner[] spawners;

    // Singleton
    private static DistanceTracker instance;
    public static DistanceTracker Instance => instance;

    // Backwards-compatible getter
    public static DistanceTracker GetInstance()
    {
        return instance;
    }

    // Event to centralize reactions to global flag changes
    public event Action<string, bool> OnGlobalFlagChanged;

    public event Action RotateRoad;

    // Cached common flags for easy access
    public bool IsRunnerRotating { get; private set; }
    public bool RunnerStage1Passed { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        CurrentDistance = 0f;

        turnDistances = new float[]{ DISTANCE_TO_FIRST_TURN, DISTANCE_TO_SECOND_TURN, DISTANCE_TO_END_GAME };
        playerRotationAnimation = PlayerRotationAnimation.Instance;
        playerMovementAcrossLevel = PlayerMovementAcrossLevel.Instance;
        spawnObstacles = SpawnObstacles.Instance;
    }

    private void OnEnable()
    {
        GlobalFlags.onFlagChanged += HandleGlobalFlagChanged;
    }

    private void OnDisable()
    {
        GlobalFlags.onFlagChanged -= HandleGlobalFlagChanged;
    }

    private void HandleGlobalFlagChanged(string flagName, bool state)
    {
        if (flagName == GlobalFlags.Flags.RUNNER_IS_ROTATING)
        {
            IsRunnerRotating = state;
        }
        else if (flagName == GlobalFlags.Flags.RUNNER_STAGE_1_PASSED)
        {
            RunnerStage1Passed = state;
        }

        OnGlobalFlagChanged?.Invoke(flagName, state);
    }

    private void Update()
    {
        //Debug.Log($"Speed: {CurrentSpeed}, Distance: {CurrentDistance}");

        if (GlobalFlags.GetFlag(GlobalFlags.Flags.GAME_OVER)) return;
        UpdateSpeed();
        CheckStages();

        //DebugInfo();
    }

    private void CheckStages()
    {
        if (CurrentDistance >= turnDistances[0] && !GlobalFlags.GetFlag(GlobalFlags.Flags.RUNNER_STAGE_1_PASSED))
        {
            DebugInfo();
            Debug.Log("First turn reached");
            // ѕри достижении дистанции запускаем событие поворота
            GlobalFlags.SetFlag(GlobalFlags.Flags.RUNNER_STAGE_1_PASSED);
            GlobalFlags.SetFlag(GlobalFlags.Flags.RUNNER_IS_ROTATING);
            RotateRoad?.Invoke();
            playerMovementAcrossLevel.CurrentDirection = Vector2.up;
            playerRotationAnimation.OnRotateRoad();


            // ћен€ем значени€, которые отвечают за сложность игры
            maxSpeed = 50f;
            MaxAcceleration = 5f;
            spawnObstacles.SpawnInterval = 2.7f;
        }
        else if (CurrentDistance >= turnDistances[1] && !GlobalFlags.GetFlag(GlobalFlags.Flags.RUNNER_STAGE_2_PASSED))
        {
            Debug.Log("Second turn reached");
            // ѕри достижении дистанции запускаем событие поворота
            GlobalFlags.SetFlag(GlobalFlags.Flags.RUNNER_STAGE_2_PASSED);
            GlobalFlags.SetFlag(GlobalFlags.Flags.RUNNER_IS_ROTATING);
            playerMovementAcrossLevel.CurrentDirection = Vector2.right;
            RotateRoad?.Invoke();
            playerRotationAnimation.OnRotateRoad();

            maxSpeed = 90f;
            MaxAcceleration = 6f;
            spawnObstacles.SpawnInterval = 2.4f;
        }
        else if (CurrentDistance >= turnDistances[2] && !GlobalFlags.GetFlag(GlobalFlags.Flags.RUNNER_STAGE_3_PASSED))
        {
            Debug.Log("Third turn reached");
            DisableSpawners();
            GlobalFlags.SetFlag(GlobalFlags.Flags.RUNNER_STAGE_3_PASSED);
            GlobalFlags.SetFlag(GlobalFlags.Flags.GAME_WIN);
            GameSceneManager.LoadNextScene();
        }
    }

    private void DisableSpawners()
    {
        foreach (var spawner in spawners)
        {
            spawner.IsAbleToSpawn = false;
        }
    }

    private void DebugInfo()
    {
        Debug.Log($"Distance: {CurrentDistance}, Speed: {CurrentSpeed}, Acceleration: {currentAcceleration}");
    }

    private void UpdateSpeed()
    {
        if (CurrentSpeed < maxSpeed) CurrentSpeed += currentAcceleration * Time.deltaTime;
        if (currentAcceleration < maxAcceleration) currentAcceleration += Time.deltaTime;
        CurrentDistance += CurrentSpeed * Time.deltaTime;
    }

    public void SetAcceleration(float newAcceleration) => currentAcceleration = newAcceleration;
    public void SetMaxSpeed(float newMaxSpeed) => maxSpeed = newMaxSpeed;
}
