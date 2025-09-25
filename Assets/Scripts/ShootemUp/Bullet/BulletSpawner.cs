using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField] private Vector2 _direction;

    [Tooltip("Задержка между спавном пуль")]
    [Min(0.1f)] // Минимальное значение
    [SerializeField] private float delay;

    [SerializeField] private BulletConfig _bulletConfig;
    [SerializeField] private BulletType currentBulletType;
    public BulletType CurrentBulletType
    {
        get => currentBulletType;
        set
        {
            currentBulletType = value;
            InitBulletGameObject();
        }
    }

    private GameObject currentBulletPrefab;
    [SerializeField] private Transform spawnBulletsPoint;
    [SerializeField] private bool isPlayerSpawner;

    private float timer;

    private void Start()
    {
        timer = 0f;
        InitBulletGameObject();
    }

    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (timer >= delay)
        {
            timer = 0f;
            SpawnBullet();
        }
    }

    private void InitBulletGameObject()
    {
        currentBulletPrefab = _bulletConfig.GetPrefab(currentBulletType);
    }

    private void SpawnBullet()
    {
        GameObject lastBullet = Instantiate(currentBulletPrefab, spawnBulletsPoint.position, Quaternion.identity); // TODO: поворот
        
        lastBullet.GetComponent<IBullet>().Initialize(_direction, isPlayerSpawner);
        //Debug.Log(lastBullet.GetComponent<IBullet>());
    }
}
