using UnityEngine;

public enum LanePosition { Left = 0, Center = 1, Right = 2 }

public class PlayerLaneController : MonoBehaviour
{
    [Header("Lane Settings")]
    [SerializeField] private GameObject[] lanePositions = new GameObject[3];
    public GameObject[] LanePositions => lanePositions;
    [SerializeField] private LanePosition startingLane = LanePosition.Center;

    public static PlayerLaneController Instance { get; private set; }

    private LanePosition currentLane;
    public LanePosition CurrentLane => currentLane;
    private PlayerInputHandler inputHandler;
    public PlayerInputHandler playerInputHandler => inputHandler;
    private PlayerMovementAnimator movementAnimator;

    public bool IsMoving => movementAnimator != null && movementAnimator.IsMoving;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        inputHandler = GetComponent<PlayerInputHandler>();
        movementAnimator = GetComponent<PlayerMovementAnimator>();
        currentLane = startingLane;
    }

    private void Start()
    {
        // ”станавливаем начальную позицию
        //movementAnimator.TeleportTo(lanePositions[(int)currentLane].transform.position);
        movementAnimator.SetTarget(lanePositions[(int)currentLane].transform);

        inputHandler.EnableVerticalInput();
    }

    private void OnEnable()
    {
        inputHandler.OnLaneChangeRequested += HandleLaneChangeRequest;
        inputHandler.OnInputModeChanged += OnInputModeChanged;
    }

    private void OnDisable()
    {
        inputHandler.OnLaneChangeRequested -= HandleLaneChangeRequest;
        inputHandler.OnInputModeChanged -= OnInputModeChanged;
    }

    private void HandleLaneChangeRequest(int direction)
    {
        //if (IsMoving) return;

        LanePosition newLane = CalculateNewLane(direction);
        if (newLane != currentLane)
        {
            ChangeLane(newLane);
        }
    }

    private LanePosition CalculateNewLane(int direction)
    {
        int newLaneIndex = (int)currentLane + direction;
        return (LanePosition)Mathf.Clamp(newLaneIndex, 0, 2);
    }

    private void ChangeLane(LanePosition newLane)
    {
        currentLane = newLane;
        // ѕередаем Transform цели, который будет автоматически обновл€тьс€ при повороте
        movementAnimator.SetTarget(lanePositions[(int)newLane].transform);
    }

    private void OnInputModeChanged(InputMode newMode)
    {
        Debug.Log($"Lane controller: Input mode changed to {newMode}");
    }

    // ѕубличные методы дл€ управлени€ извне
    public void ChangeInputMode(InputMode mode) => inputHandler.SetInputMode(mode);
    public void DisableMovement() => inputHandler.DisableInput();
    public void EnableMovement(InputMode mode) => inputHandler.SetInputMode(mode);
}