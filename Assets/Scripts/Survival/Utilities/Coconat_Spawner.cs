using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coconat_Spawner : MonoBehaviour
{
    public GameObject cocoPrefab;
    public float TimeToRespawn;
    private float TimerToRespawn;


    private void Start()
    {
        TimerToRespawn = TimeToRespawn;
    }
    private void Spawn_Coconat()
    {
        Instantiate(cocoPrefab, transform.position, Quaternion.identity);
    }

    private void Update()
    {
        bool coconutExists = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.5f); 
        foreach (var col in colliders)
        {
            if (col.CompareTag("Coconut"))
            {
                coconutExists = true;
                break;
            }
        }
        if (TimerToRespawn <= 0 && !coconutExists)
        {
            Spawn_Coconat();
            TimerToRespawn = TimeToRespawn;
        }
        else 
        {
            TimerToRespawn -= Time.deltaTime;
        }
    }

}
