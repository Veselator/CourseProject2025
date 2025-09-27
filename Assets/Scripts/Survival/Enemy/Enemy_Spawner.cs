using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject bossPrefab;
    public float TimeToSpawn = 2f;
    public float TimeToSpawnBoss = 30f;
    private float BossSpawnrTime = 0;
    private float SpawnTimer = 0;
    private int maxEnemyOnTheField = 1;

    
    private void SpawnEnemy() 
    {
        
        int enemCount = GameObject.FindGameObjectsWithTag(enemyPrefab.tag).Length;
        if (enemCount < maxEnemyOnTheField)
        {
            for (int k = 0; k < (maxEnemyOnTheField - enemCount); ++k)
            {
                if (SpawnTimer <= 0)
                {
                    float rand_x = Random.Range(-17.5f, 17.5f);
                    float rand_y = Random.Range(-10f, 10f);
                    Instantiate(enemyPrefab, new Vector3(rand_x, rand_y, 0), Quaternion.identity);
                    SpawnTimer = TimeToSpawn;
                }
                else 
                { 
                SpawnTimer -= Time.deltaTime;
                }
            }
        }
    }
    private void Check_For_Kills() 
    {
        if (KillCounter.Instance.count == maxEnemyOnTheField * 2) 
        {
            maxEnemyOnTheField++;
        }
    }
    private void SpawnBoss() 
    {
        
        float rand_x = Random.Range(-17.5f, 17.5f);
        float rand_y = Random.Range(-10f, 10f);
        GameObject bossInstance = Instantiate(bossPrefab, new Vector3(rand_x, rand_y, 0), Quaternion.identity);

       
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
