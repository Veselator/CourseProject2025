using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRoad : MonoBehaviour
{
    [SerializeField] private GameObject roadPrefab;
    [SerializeField] private GameObject[] turnPrefabs = new GameObject[2];
    private int _currentTurnIndex = 0;
    [SerializeField] private float distanceBetweenRoads = 10f;
    [SerializeField] private int initialRoadCount = 5;
    [SerializeField] private GameObject _player;

    private Transform _playerTransform;
    private Vector3 lastSpawnedPosition;
    private List<GameObject> spawnedRoads = new List<GameObject>();
    [SerializeField] private const float roadDespawnDistance = 30f;
    private PlayerMovementAcrossLevel _playerMovement;
    private DistanceTracker _distanceTracker;

    public GameObject LastSpawnedTurn { get; private set; }

    public static SpawnRoad Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        _playerMovement = PlayerMovementAcrossLevel.Instance;
        _distanceTracker = DistanceTracker.Instance;
        _playerTransform = _playerMovement.transform;

        _distanceTracker.RotateRoad += OnRotateRoad;

        lastSpawnedPosition = transform.position;
        for (int i = 0; i < initialRoadCount; i++)
        {
            SpawnNextRoad();
        }
    }

    private void OnDestroy()
    {
        _distanceTracker.RotateRoad -= OnRotateRoad;
    }

    private void OnRotateRoad()
    {
        Debug.Log("SpawnRoad: OnRotateRoad called, spawning turn.");
        SpawnTurn();
    }

    private void Update()
    {
        bool shouldSpawn = Vector3.Distance(_playerTransform.position, lastSpawnedPosition) < distanceBetweenRoads * 2;
        
        if (!shouldSpawn) return;
        SpawnNextRoad();
        DespawnOldRoads();
    }

    private void SpawnNextRoad()
    {
        SpawnPart(roadPrefab, Vector2Rotation(_playerMovement.CurrentDirection));
    }

    private void SpawnTurn()
    {
        LastSpawnedTurn = SpawnPart(turnPrefabs[_currentTurnIndex], Vector2Rotation(_playerMovement.CurrentDirection));
        _currentTurnIndex++;
    }

    private Quaternion GetRoadDirectionBasedOnCurrentMovement(Vector3 inputVector)
    {
        switch (inputVector)
        {
            case Vector3 v when v == Vector3.up:
                return Quaternion.Euler(0f, 0f, -180f);
            case Vector3 v when v == Vector3.right:
                return Quaternion.Euler(0f, 0f, 0f);
            default:
                return Quaternion.identity;
        }
    }

    private Quaternion Vector2Rotation(Vector3 inputVector)
    {
        switch(inputVector)
        {
            case Vector3 v when v == Vector3.up:
                return Quaternion.Euler(0f, 0f, 90f);
            case Vector3 v when v == Vector3.down:
                return Quaternion.Euler(0f, 0f, -90f);
            case Vector3 v when v == Vector3.left:
                return Quaternion.Euler(0f, 0f, 180f);
            case Vector3 v when v == Vector3.right:
                return Quaternion.Euler(0f, 0f, 0f);
            default:
                return Quaternion.identity;
        }
    }

    private GameObject SpawnPart(GameObject objPrefab, Quaternion currentRotation)
    {
        GameObject obj = Instantiate(objPrefab, lastSpawnedPosition, currentRotation);
        spawnedRoads.Add(obj);
        lastSpawnedPosition = obj.transform.Find("Out").transform.position;

        return obj;
    }

    private void DespawnOldRoads()
    {
        if (spawnedRoads.Count == 0) return;
        if (Vector3.Distance(spawnedRoads[0].transform.position, _player.transform.position) > roadDespawnDistance)
        {
            Destroy(spawnedRoads[0]);
            spawnedRoads.RemoveAt(0);
        }

        //for (int i = spawnedRoads.Count - 1; i >= 0; i--)
        //{
        //    var piece = spawnedRoads[i];
        //    if (piece == null)
        //    {
        //        spawnedRoads.RemoveAt(i);
        //        continue;
        //    }
        //    if (piece.transform == null)
        //    {
        //        Debug.LogWarning($"SpawnRoad: spawned piece at index {i} has no transform, removing.");
        //        spawnedRoads.RemoveAt(i);
        //        continue;
        //    }
        //    if (Vector3.Distance(_playerTransform.position, piece.transform.position) > roadDespawnDistance)
        //    {
        //        Destroy(piece);
        //        spawnedRoads.RemoveAt(i);
        //    }
        //}
    }
}
