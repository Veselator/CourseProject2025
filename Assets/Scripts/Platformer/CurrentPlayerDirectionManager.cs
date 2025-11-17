using System;
using UnityEngine;

public class CurrentPlayerDirectionManager : MonoBehaviour
{
    [SerializeField] private ObjectFlipperManager _flipperManager;
    private Vector2 _currentDirection = new Vector2(1f, 0f);
    public Vector2 CurrentDirection => _currentDirection;

    public static CurrentPlayerDirectionManager Instance { get; private set; }
    public event Action<Vector2> OnDirectionChanged;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _flipperManager.OnFlipped += ChangeDirection;
    }

    private void OnDestroy()
    {
        _flipperManager.OnFlipped -= ChangeDirection;
    }

    private void ChangeDirection()
    {
        _currentDirection *= -1;
        OnDirectionChanged?.Invoke(_currentDirection);
    }
}
