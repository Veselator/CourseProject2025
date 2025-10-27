using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private LivesManager _livesManager;
    [SerializeField] private Transform _spawnPoint;

    private void Start()
    {
        _livesManager = LivesManager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent<PlayerPlatformerHandler>(out _)) return;
        _livesManager.SetSpawnPoint(_spawnPoint);
    }
}
