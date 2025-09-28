using System;
using UnityEngine;

public class MoneySpawner : BaseSpawner
{
    private GameObject[] lanes;
    PlayerLaneController playerLaneController;

    public static MoneySpawner Instance { get; private set; }

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
        GameObject obstacleObject = targetPrefabs[0];
        Vector3 obstaclePosition = lanes[randomLaneIndex].transform.position + lanes[randomLaneIndex].transform.right * distanceOfSpawning;
        obstaclePosition.z = -1;

        obstacleObject = Instantiate(obstacleObject, obstaclePosition, Quaternion.identity);
        spawnedObjects.Add(obstacleObject);

        obstacleObject.GetComponent<ObstacleInfo>().Init(randomLaneIndex);
    }

    private int GetRandomLaneIndex()
    {
        return UnityEngine.Random.Range(0, lanes.Length);
    }
}
