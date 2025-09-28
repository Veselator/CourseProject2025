using System;
using UnityEngine;

public class SpawnObstacles : BaseSpawner
{
    [SerializeField] protected GameObject[] targetPrefabsVertical;
    private GameObject[] lanes;
    private const float CHANCE_TO_SPAWN_IN_PRIORITY_LANE = 0.40f; // 40% шанс спавна в приоритетной полосе

    PlayerLaneController playerLaneController;
    // Первый int - номер линии
    // Второй InputMode - текущий режим ввода
    public Action<int, InputMode> OnObstacleSpawned;

    public static SpawnObstacles Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        playerLaneController = PlayerLaneController.Instance;
        lanes = playerLaneController.LanePositions;
    }

    protected override void SpawnTarget()
    {
        if (targetPrefabs == null) return;

        int randomLaneIndex = GetRandomLaneIndex();
        GameObject obstacleObject = GetRandomPrefab();
        Vector3 obstaclePosition = lanes[randomLaneIndex].transform.position + lanes[randomLaneIndex].transform.right * distanceOfSpawning;
        obstaclePosition.z = -1;

        obstacleObject = Instantiate(obstacleObject, obstaclePosition, Quaternion.identity);
        spawnedObjects.Add(obstacleObject);

        obstacleObject.GetComponent<ObstacleInfo>().Init(randomLaneIndex);

        OnObstacleSpawned?.Invoke(randomLaneIndex, playerLaneController.playerInputHandler.CurrentInputMode);
    }

    private int GetRandomLaneIndex()
    {
        if (UnityEngine.Random.Range(0f, 1f) < CHANCE_TO_SPAWN_IN_PRIORITY_LANE)
            return (int)playerLaneController.CurrentLane;
        return UnityEngine.Random.Range(0, lanes.Length);
    }

    protected override GameObject GetRandomPrefab()
    {
        if (targetPrefabs == null) return null;
        switch (playerLaneController.playerInputHandler.CurrentInputMode)
        {
            case InputMode.Horizontal:
                return targetPrefabsVertical[UnityEngine.Random.Range(0, targetPrefabsVertical.Length - 1)];
            case InputMode.Vertical:
            default:
                return targetPrefabs[UnityEngine.Random.Range(0, targetPrefabs.Length - 1)];
        }
    }
}
