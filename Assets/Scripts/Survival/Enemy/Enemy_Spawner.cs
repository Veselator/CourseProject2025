using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy_Spawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject bossPrefab;
    public float TimeToSpawn = 2f;
    public float TimeToSpawnBoss = 30f;
    private float BossSpawnrTime = 0;
    private float SpawnTimer = 0;
    [SerializeField] private float spawnDelay;
    private int maxEnemyOnTheField = 2;

    private Transform[] spawnPositions;
    private KillCounter killCounter;

    private void Start()
    {
        killCounter = KillCounter.Instance;
        InitSpawnPositions();
        ApplyDelay();
    }

    private void ApplyDelay()
    {
        BossSpawnrTime -= spawnDelay;
        SpawnTimer = spawnDelay;
    }

    private void InitSpawnPositions()
    {
        // Исключаем текущий Transform из всех Transform
        spawnPositions = GetComponentsInChildren<Transform>().Where(t => t != transform).ToArray();
    }

    private void SpawnEnemy() 
    {
        
        int enemCount = GameObject.FindGameObjectsWithTag(enemyPrefab.tag).Length;
        if (enemCount < maxEnemyOnTheField)
        {
            for (int k = 0; k < (maxEnemyOnTheField - enemCount); ++k)
            {
                if (SpawnTimer <= 0)
                {
                    Instantiate(enemyPrefab, GetRandomPosition(), Quaternion.identity);
                    SpawnTimer = TimeToSpawn;
                }
                else 
                { 
                    SpawnTimer -= Time.deltaTime;
                }
            }
        }
    }

    private Vector3 GetRandomPosition()
    {
        return spawnPositions[Random.Range(0, spawnPositions.Length - 1)].position;
    }

    private void Check_For_Kills() 
    {
        if (killCounter.count == maxEnemyOnTheField * 2) 
        {
            maxEnemyOnTheField++;
        }
    }
    private void SpawnBoss() 
    {
        GameObject bossInstance = Instantiate(bossPrefab, GetRandomPosition(), Quaternion.identity);

       
        Boss_Health bossHealth = bossInstance.GetComponent<Boss_Health>();
        if (bossHealth != null)
        {
            bossHealth.Gain_Additional_Health();
        }

    }
    private void Update()
    {
        BossSpawnrTime += Time.deltaTime;

        if (BossSpawnrTime >= TimeToSpawnBoss) 
        { 
            SpawnBoss();
            BossSpawnrTime = 0;
        }

        Check_For_Kills();
        SpawnEnemy();
      
    }
}
