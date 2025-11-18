using System;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private LivesManager _livesManager;
    [SerializeField] private Transform _spawnPoint;

    public event Action OnPlayerEnter;

    private void Start()
    {
        _livesManager = LivesManager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent<PlayerPlatformerHandler>(out _)) return;
        OnPlayerEnter?.Invoke();
        _livesManager.SetSpawnPoint(_spawnPoint);
    }
}
