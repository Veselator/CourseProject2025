using UnityEngine;

public class PlayerMovementHandler : MonoBehaviour
{
    private PlayerInput _playerInput;
    private IMovement _movement;

    [SerializeField] private Box clampingBox;

    public static PlayerMovementHandler Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        // ���� - ����
        // ����� ���� ����� �������� �� VContainer

        _playerInput = PlayerInput.Instance;
        _movement = GetComponent<RigidbodyMovement>();
        _movement.Init(Vector2.zero);
        _movement.SetIsClamped(true);
        _movement.SetClampBorders(clampingBox.startPoint, clampingBox.endPoint);
    }

    private void Update()
    {
        HandleMovingInput();
    }

    private void HandleMovingInput()
    {
        Vector2 movementVector = _playerInput.GetMovementVector();
        _movement.ChangeVelocity(movementVector);
    }
}
