using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRotating : MonoBehaviour
{
    [SerializeField] private GameObject roadPrefab;
    [SerializeField] private GameObject povorotPrefab;

    [SerializeField] private int tilesBeforeTurn = 4;
    [SerializeField] private int tilesAfterTurn = 4;

    private Vector3 currentDirection = Vector3.right;
    private Vector3 lastSpawnedPosition;

    private void Start()
    {
        // Start spawning from this object's position
        lastSpawnedPosition = transform.position;
        GeneratePath();
    }

    private void GeneratePath()
    {
        // Spawn a number of road prefabs in the current direction
        for (int i = 0; i < tilesBeforeTurn; i++)
        {
            SpawnRoad();
        }

        // Spawn the turn prefab at the end of the straight segment
        SpawnTurn();

        // Change direction up and spawn more road prefabs
        currentDirection = Vector3.up;

        for (int i = 0; i < tilesAfterTurn; i++)
        {
            SpawnRoad();
        }
    }

    private void SpawnRoad()
    {
        Quaternion rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(currentDirection.y, currentDirection.x) * Mathf.Rad2Deg);
        GameObject lastRoad = Instantiate(roadPrefab, lastSpawnedPosition, rotation, transform);
        lastSpawnedPosition = lastRoad.transform.Find("Out").transform.position;
    }

    private void SpawnTurn()
    {
        Quaternion rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(currentDirection.y, currentDirection.x) * Mathf.Rad2Deg);
        GameObject lastRoad = Instantiate(povorotPrefab, lastSpawnedPosition, rotation, transform);
        lastSpawnedPosition = lastRoad.transform.Find("Out").transform.position;
    }
}
