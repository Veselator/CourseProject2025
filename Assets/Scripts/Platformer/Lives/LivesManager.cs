using UnityEngine;

public class LivesManager : MonoBehaviour
{
    private IHealth _health;
    private int _lives = 3;
    [SerializeField] private Transform _lastSpawnPoint;
    [SerializeField] private Transform _player;

    public static LivesManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        _health = GetComponent<IHealth>();
        _health.OnDeath += HandleDeath;
    }

    private void OnDestroy()
    {
        _health.OnDeath -= HandleDeath;
    }

    public void SetSpawnPoint(Transform _newSpawnPointTransform)
    {
        _lastSpawnPoint = _newSpawnPointTransform;
    }

    private void HandleDeath()
    {
        _lives--;
        if (_lives == 0)
        {
            // Усьо
            // TODO: экран смерти
        }
        else
        {
            _player.position = _lastSpawnPoint.position;
            _health.ResetHealth();
        }
    }
}
