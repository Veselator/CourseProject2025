using System;
using UnityEngine;

public class PlayerMovementHandler : MonoBehaviour
{
    // Необходим для связки Input и Movement
    // Тут же можно добавить обработку прыжка

    protected PlayerInput _playerInput;
    protected IMovement _movement;

    [SerializeField] private Box clampingBox;

    public Vector2 MovementVector => _playerInput.GetMovementVector();
    public static PlayerMovementHandler Instance { get; private set; }

    // Для дочерних классов
    protected virtual bool IsHandleAdditionalThings { get; } = false;
    public bool IsMovementBlocked { get; set; } = false; // Для кат-сцен
    protected Vector2 __zeroVector = Vector2.zero;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        _playerInput = PlayerInput.Instance;
        _movement = GetComponent<IMovement>();
        _movement.Init(Vector2.zero);
        _movement.SetIsClamped(true);
        _movement.SetClampBorders(clampingBox.startPoint, clampingBox.endPoint);
    }

    private void Update()
    {
        if (GlobalFlags.GetFlag(Flags.GameOver)) return;
        if (IsHandleAdditionalThings) HandleAdditionalThings();
        HandleInput();
    }

    protected virtual void HandleInput()
    {
        // Для того что-бы можно было переопределять в потомках
        HandleMovingInput();
    }

    protected virtual void HandleAdditionalThings()
    {
        Debug.Log("Child classes only");
    }

    protected virtual void HandleMovingInput()
    {
        if(!IsMovementBlocked) _movement.ChangeVelocity(MovementVector);
        else _movement.ChangeVelocity(__zeroVector);
    }
}
