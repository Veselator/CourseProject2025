using System;
using System.Collections;
using UnityEngine;

public class PlatformerObstaclesSpawner : MonoBehaviour
{
    // Внешние зависимости
    [SerializeField] private BossPhasesManager _bossPhasesManager;
    [SerializeField] private Transform[] _spawnPoints = new Transform[2];
    [SerializeField] private SpawnConfiguration[] _spawnConfigurations = new SpawnConfiguration[2];
    [SerializeField] private GameObject[] _obstaclePrefabs;

    // Внутренние параметры спавна
    private SpawnConfiguration _currentSpawnConfiguration;
    private int _currentNumOfObstacles;
    private bool _isSpawning = false;
    [SerializeField] private float _delayBeforeSpawning = 1.2f;

    private void Awake()
    {
        _bossPhasesManager.OnPhaseStarted += CheckPhaseStarted;
        _bossPhasesManager.OnPhaseEnded += CheckPhaseEnded;
    }

    private void Start()
    {
        if (_spawnPoints.Length != 2) Debug.LogError("В массиве _spawnPoints должно быть 2 элемента!");
    }

    private void OnDestroy()
    {
        _bossPhasesManager.OnPhaseStarted -= CheckPhaseStarted;
        _bossPhasesManager.OnPhaseEnded -= CheckPhaseEnded;
    }

    private void CheckPhaseEnded(PhaseID phaseId)
    {
        if (_isSpawning)
        {
            _isSpawning = false;
            StopAllCoroutines();
        }
    }

    private void CheckPhaseStarted(PhaseID phaseId)
    {
        if (phaseId == PhaseID.Third || phaseId == PhaseID.None) return; // Неинтересно

        _currentSpawnConfiguration = _spawnConfigurations[(int)phaseId];

        if (_currentSpawnConfiguration.fromSpawnDelay == 0f || _currentSpawnConfiguration.toSpawnDelay == 0f)
        {
            Debug.LogError("Нулевой spawnDelay?! Что-бы компьютер взорвался от бесконечного спавна?!");
            return;
        }

        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        yield return new WaitForSeconds(_delayBeforeSpawning);
        // Начинаем спавнить препятствия
        _isSpawning = true;

        // Проходимся по всем кучкам врагов

        // Можно было заранее сгенерировать pool препятствий
        // И потом просто "брать" из кучки
        // Но конкретно для этой архитектуры такая оптимизация излишняя
        // Расчёты не тяжёлые, общее количество объектов <100
        do
        {
            for (int i = 0; i < _currentSpawnConfiguration.numOfBunches; i++)
            {
                // Заходим в кучу
                float currentInterpolation = (float)i / _currentSpawnConfiguration.numOfBunches;
                int currentNumOfObstaclesPerBunch = (int)Mathf.Lerp(_currentSpawnConfiguration.fromNumOfObstaclesPerBunch, _currentSpawnConfiguration.toNumOfObstaclerPerBunch, currentInterpolation);
                for (int j = 0; j < currentNumOfObstaclesPerBunch; j++)
                {
                    // Получаем текущее случайно сгенерированное число
                    int currentRnd = UnityEngine.Random.Range(0, 100);

                    SpawnLocation currentSpawnLocation = (SpawnLocation)(currentRnd % 3);
                    GameObject currentPrefab = _obstaclePrefabs[currentRnd % _obstaclePrefabs.Length];

                    float currentSpeed = Mathf.Lerp(_currentSpawnConfiguration.fromObstaclesSpeed, _currentSpawnConfiguration.toObstacleSpeed, currentInterpolation);
                    float currentDelay = Mathf.Lerp(_currentSpawnConfiguration.fromSpawnDelay, _currentSpawnConfiguration.toSpawnDelay, currentInterpolation);

                    // Создаём препятствие
                    CreateNewObstacle(currentPrefab, currentSpawnLocation, currentSpeed);
                    yield return new WaitForSeconds(currentDelay);
                }

                float currentDelayBetweenBunches = Mathf.Lerp(_currentSpawnConfiguration.fromDelayBetweenBunches, _currentSpawnConfiguration.toDelayBetweenBunches, currentInterpolation);
                yield return new WaitForSeconds(currentDelayBetweenBunches);
            }
        }
        while (_currentSpawnConfiguration.isEndless);

        _isSpawning = false;
    }

    public void HandleObstacleDestroyed()
    {
        _currentNumOfObstacles--;
        if (_isSpawning || _currentSpawnConfiguration.isEndless) return;
        if (_currentNumOfObstacles <= 0) _bossPhasesManager.TryToEndPhase(); // Волна усё
    }

    // Можно было реализовать как отдельный класс-фабрику
    // Но для данной задачи - over-engineering
    private void CreateNewObstacle(GameObject spawnPrefab, SpawnLocation spawnLocation, float speed)
    {
        if (spawnLocation == SpawnLocation.Left || spawnLocation == SpawnLocation.Both)
        {
            SpawnObstacle(spawnPrefab, speed, _spawnPoints[0], _spawnPoints[1]);
        }

        if (spawnLocation == SpawnLocation.Right || spawnLocation == SpawnLocation.Both)
        {
            SpawnObstacle(spawnPrefab, speed, _spawnPoints[1], _spawnPoints[0]);
        }
    }

    private void SpawnObstacle(GameObject spawnPrefab, float speed, Transform from, Transform to)
    {
        GameObject spawnedPrefab = Instantiate(spawnPrefab, from.position, Quaternion.identity);
        spawnedPrefab.GetComponent<MovingObstacle>().Init(this, speed, from, to);
    }
}

[Serializable]
public struct SpawnConfiguration
{
    // Все значения интерполируются
    // Для повышения сложности
    [Header("Время между спавном препятствий в рамках одного набора")]
    public float fromSpawnDelay, toSpawnDelay;
    [Header("Скорость препятствий")]
    public float fromObstaclesSpeed, toObstacleSpeed;
    [Header("Количество препятствий в одной группе")]
    public int fromNumOfObstaclesPerBunch, toNumOfObstaclerPerBunch;
    [Header("Время между спавном групп")]
    public float fromDelayBetweenBunches, toDelayBetweenBunches;
    [Header("Количество групп")]
    public int numOfBunches;
    [Header("Спавним ли бесконечно - пока какое-то условие не остановит спан")]
    public bool isEndless; // Для второй фазы
}

public enum SpawnLocation
{
    Left,
    Right,
    Both
}