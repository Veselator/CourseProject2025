using System;
using UnityEngine;

public class LivesManager : MonoBehaviour, IReadableValue
{
    private IHealth _health;
    [SerializeField] private int MaxLives = 3;
    private int _lives;
    [SerializeField] private Transform _lastSpawnPoint;
    [SerializeField] private RigidbodyPlatformerMovement _playerMovement; // Что-бы сбросить движение
    [SerializeField] private Transform _player;

    public event Action<float> OnValueChanged;

    public static LivesManager Instance { get; private set; }

    public float Value => _lives;

    private void Awake()
    {
        _lives = MaxLives;
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

    // Да, это нарушает принцип SRP - мененджер жизней также отвечает и за сохранение точек спавна
    // И что ты мне сделаешь, а?
    public void SetSpawnPoint(Transform _newSpawnPointTransform)
    {
        _lastSpawnPoint = _newSpawnPointTransform;
    }

    public void AddLives(int count = 1)
    {
        _lives += count;
        _lives = Mathf.Max(MaxLives, _lives); // Что-бы здоровье не выходило за границы
        OnValueChanged?.Invoke(Value);
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
            _playerMovement?.ResetMovement();
            _health?.ResetHealth();
            OnValueChanged?.Invoke(Value);
        }
    }
}
