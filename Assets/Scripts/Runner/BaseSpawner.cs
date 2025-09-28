using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSpawner : MonoBehaviour
{
    // Класс для описания сущности, которая создаёт другие сущности с определённым интервалом

    // Target - object type that we spawn
    [SerializeField] protected GameObject[] targetPrefabs;
    [SerializeField] protected float spawnInterval = 4f;
    [SerializeField] protected float distanceOfSpawning = 10f;
    public bool IsAbleToSpawn = true;
    protected List<GameObject> spawnedObjects = new List<GameObject>();

    private float _timer;

    public float SpawnInterval
    {
        get
        {
            return spawnInterval;
        }
        set
        {
            spawnInterval = Mathf.Max(value, 0f);
        }
    }

    private void Update()
    {
        if (!IsAbleToSpawn) return;
        _timer += Time.deltaTime;
        if (_timer >= spawnInterval)
        {
            _timer = 0f;
            SpawnTarget();
            CheckToDespawn();
        }
    }

    protected abstract void SpawnTarget();
    protected virtual void CheckToDespawn()
    {
        if (spawnedObjects.Count == 0) return;

        if (spawnedObjects[0] == null)
        {
            spawnedObjects.RemoveAt(0);
            return;
        }

        if (Vector3.Distance(transform.position, spawnedObjects[0].transform.position) > distanceOfSpawning * 3)
        {
            Destroy(spawnedObjects[0]);
            spawnedObjects.RemoveAt(0);
        }
    }

    protected virtual GameObject GetRandomPrefab()
    {
        if (targetPrefabs == null) return null;
        if (targetPrefabs.Length == 1) return targetPrefabs[0];

        return targetPrefabs[Random.Range(0, targetPrefabs.Length - 1)];
    }

    public void SetSpawnInterval(float e) => spawnInterval = e;
}
