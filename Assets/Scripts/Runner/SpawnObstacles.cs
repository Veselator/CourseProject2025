using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObstacles : MonoBehaviour
{
    [SerializeField] private GameObject obstaclePrefab;
    private GameObject[] lanes;
    [SerializeField] private float spawnInterval = 4f;
    [SerializeField] private float obstacleDistanceOfSpawning = 20f;
    private const float CHANCE_TO_SPAWN_IN_PRIORITY_LANE = 0.40f; // 40% шанс спавна в приоритетной полосе

    PlayerLaneController playerLaneController;
    private float timer = 0f;

    private List<GameObject> spawnedObstacles = new List<GameObject>();

    private DistanceTracker _distanceTracker;
    // Первый int - номер линии
    // Второй InputMode - текущий режим ввода
    public Action<int, InputMode> OnObstacleSpawned;

    public static SpawnObstacles Instance { get; private set; }

    public float SpawnInterval
    {
        get => spawnInterval;
        set { if (value > 0f) spawnInterval = value; }
    }

    public float ObstacleDistanceOfSpawning
    {
        get => obstacleDistanceOfSpawning;
        set { if (value > 0f) obstacleDistanceOfSpawning = value; }
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        playerLaneController = PlayerLaneController.Instance;
        lanes = playerLaneController.LanePositions;
        _distanceTracker = DistanceTracker.Instance;
    }

    public void Update()
    {
        if (GlobalFlags.GetFlag(GlobalFlags.Flags.GAME_OVER)) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            if (_distanceTracker.IsRunnerRotating) return;
            SpawnObstacle();
            CheckObstaclesToDespawn();
        }
    }

    public void SetSpawnInterval(float e) => spawnInterval = e;

    private void SpawnObstacle()
    {
        if (obstaclePrefab == null) return;

        int randomLaneIndex = GetRandomLaneIndex();
        GameObject obstacleObject = GetRandomObstacle();
        Vector3 obstaclePosition = lanes[randomLaneIndex].transform.position + lanes[randomLaneIndex].transform.right * obstacleDistanceOfSpawning;
        obstaclePosition.z = -1;

        obstacleObject = Instantiate(obstacleObject, obstaclePosition, Quaternion.identity);
        spawnedObstacles.Add(obstacleObject);

        obstacleObject.GetComponent<ObstacleInfo>().Init(randomLaneIndex);

        OnObstacleSpawned?.Invoke(randomLaneIndex, playerLaneController.playerInputHandler.CurrentInputMode);
        //if (obstacleObject.TryGetComponent<ObstacleInfo>(out ObstacleInfo obstacleInfo))
        //{
        //    obstacleInfo.LaneIndex = randomLaneIndex;
        //}
        //else
        //{
        //    Debug.LogError("Obstacle prefab is missing ObstacleInfo component.");
        //}
    }

    private GameObject GetRandomObstacle()
    {
        return obstaclePrefab;
    }

    private void CheckObstaclesToDespawn()
    {
        if (spawnedObstacles.Count == 0) return;

        if (spawnedObstacles[0] == null)
        {
            spawnedObstacles.RemoveAt(0);
            return;
        }

        if (Vector3.Distance(transform.position, spawnedObstacles[0].transform.position) > obstacleDistanceOfSpawning * 2)
        {
            Destroy(spawnedObstacles[0]);
            spawnedObstacles.RemoveAt(0);
        }
    }

    private int GetRandomLaneIndex()
    {
        if (UnityEngine.Random.Range(0f, 1f) < CHANCE_TO_SPAWN_IN_PRIORITY_LANE)
            return (int)playerLaneController.CurrentLane;
        return UnityEngine.Random.Range(0, lanes.Length);
    }

    private GameObject LaneIndexToGameObject(LanePosition landeIndex)
    {
        return LaneIndexToGameObject((int)landeIndex);
    }

    private GameObject LaneIndexToGameObject(int landeIndex)
    {
        return lanes[landeIndex];
    }
}
