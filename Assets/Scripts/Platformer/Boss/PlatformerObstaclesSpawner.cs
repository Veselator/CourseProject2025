using System.Collections;
using System.Collections.Generic;
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

    private void Awake()
    {
        _bossPhasesManager.OnPhaseStarted += CheckPhaseStarted;
    }

    private void Start()
    {
        if (_spawnPoints.Length != 2) Debug.LogError("В массиве _spawnPoints должно быть 2 элемента!");
    }

    private void OnDestroy()
    {
        _bossPhasesManager.OnPhaseStarted -= CheckPhaseStarted;
    }

    private void CheckPhaseStarted(PhaseID phaseId)
    {
        if (phaseId == PhaseID.Third || phaseId == PhaseID.None) return; // Неинтересно

        _currentSpawnConfiguration = _spawnConfigurations[(int)phaseId];

        if (_currentSpawnConfiguration.spawnDelay == 0f)
        {
            Debug.LogError("Нулевой spawnDelay?! Что-бы компьютер взорвался от бесконечного спавна?!");
            return;
        }

        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        // Начинаем спавнить препятствия
        _isSpawning = true;

        // Проходимся по всем кучкам врагов

        // Можно было заранее сгенерировать pool препятствий
        // И потом просто "брать" из кучки
        // Но конкретно для этой архитектуры такая оптимизация излишняя
        // Расчёты не тяжёлые, общее количество объектов <100
        for (int i = 0; i < _currentSpawnConfiguration.numOfBunches; i++)
        {
            // Заходим в кучу
            for (int j = 0; j < _currentSpawnConfiguration.numOfObstaclesPerBunch; j++)
            {
                // Получаем текущее случайно сгенерированное число
                int currentRnd = Random.Range(0, 100);

                SpawnLocation currentSpawnLocation = (SpawnLocation)(currentRnd % 3);
                GameObject currentPrefab = _obstaclePrefabs[currentRnd % _obstaclePrefabs.Length];

                // Создаём препятствие
                CreateNewObstacle(currentPrefab, currentSpawnLocation, _currentSpawnConfiguration.obstaclesSpeed);
                yield return new WaitForSeconds(_currentSpawnConfiguration.spawnDelay);
            }

            yield return new WaitForSeconds(_currentSpawnConfiguration.delayBetweenBunches);
        }

        _isSpawning = false;
    }

    public void HandleObstacleDestroyed()
    {
        _currentNumOfObstacles--;
        if (_isSpawning) return;
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

public struct SpawnConfiguration
{
    public float spawnDelay;
    public float obstaclesSpeed;
    public int numOfObstaclesPerBunch;
    public int numOfBunches;
    public float delayBetweenBunches;
}

public enum SpawnLocation
{
    Left,
    Right,
    Both
}