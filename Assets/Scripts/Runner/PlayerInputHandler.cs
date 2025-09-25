using System;
using UnityEngine;

public enum InputMode { None, Horizontal, Vertical }

public class PlayerInputHandler : MonoBehaviour
{
    public event Action<int> OnLaneChangeRequested;
    public event Action<InputMode> OnInputModeChanged;
    public event Action OnJumpRequested;
    private bool jumpPressed = false;

    private PlayerInput playerInput;
    private bool positivePressed = false;
    private bool negativePressed = false;

    [SerializeField] private float inputThreshold = 0.5f;

    public InputMode CurrentInputMode { get; private set; }
    public static PlayerInputHandler Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if (CurrentInputMode == InputMode.None ||
            GlobalFlags.GetFlag(GlobalFlags.Flags.RUNNER_IS_ROTATING) ||
            GlobalFlags.GetFlag(GlobalFlags.Flags.BLOCK_PLAYER_MOVING) ||
            GlobalFlags.GetFlag(GlobalFlags.Flags.GAME_OVER)) return;

        HandleMovingButtons();
        HandleJump();
    }

    private void OnEnable()
    {
        GlobalFlags.onFlagChanged += OnGlobalFlagChanged;
    }

    private void OnDisable()
    {
        GlobalFlags.onFlagChanged -= OnGlobalFlagChanged;
    }

    private void OnGlobalFlagChanged(string flag, bool state)
    {
        if (flag == GlobalFlags.Flags.RUNNER_STAGE_1_PASSED && state)
        {
            // ���� ������ ������ ����, ��������� �������������� ����
            EnableHorizontalInput();
        }
        if (flag == GlobalFlags.Flags.RUNNER_STAGE_2_PASSED && state)
        {
            // ���� ������ ������ ����, ��������� ������������ ����
            EnableVerticalInput();
        }
    }

    public void SetInputMode(InputMode mode)
    {
        if (CurrentInputMode != mode)
        {
            InputMode previousMode = CurrentInputMode;
            CurrentInputMode = mode;

            // ���������� ��������� ������� ������ ��� ����� ������
            ResetInputState();

            OnInputModeChanged?.Invoke(mode);

            Debug.Log($"Input mode changed from {previousMode} to {mode}");
        }
    }

    private void ResetInputState()
    {
        positivePressed = false;
        negativePressed = false;
    }

    private void HandleMovingButtons()
    {
        Vector2 movementVector = playerInput.GetMovementVector();

        float inputValue = GetInputValueByMode(movementVector);
        //Debug.Log(GetCurrentInputInfo());

        bool positiveCurrently = inputValue > inputThreshold;
        bool negativeCurrently = inputValue < -inputThreshold;

        if (positiveCurrently && !positivePressed)
        {
            OnLaneChangeRequested?.Invoke(1);
        }

        if (negativeCurrently && !negativePressed)
        {
            OnLaneChangeRequested?.Invoke(-1);
        }

        positivePressed = positiveCurrently;
        negativePressed = negativeCurrently;
    }

    private void HandleJump()
    {
        bool isJumpCurrently = playerInput.IsHitButtonPressed();

        if (isJumpCurrently && !jumpPressed)
        {
            OnJumpRequested?.Invoke();
        }

        jumpPressed = isJumpCurrently;
    }

    private float GetInputValueByMode(Vector2 movementVector)
    {
        return CurrentInputMode switch
        {
            InputMode.Horizontal => movementVector.x,
            InputMode.Vertical => -movementVector.y,
            InputMode.None => 0f,
            _ => 0f
        };
    }

    // ��������� ������ ��� �������� ����������
    public void DisableInput() => SetInputMode(InputMode.None);
    public void EnableHorizontalInput() => SetInputMode(InputMode.Horizontal);
    public void EnableVerticalInput() => SetInputMode(InputMode.Vertical);

    // ��� �������
    public string GetCurrentInputInfo()
    {
        Vector2 movement = playerInput.GetMovementVector();
        return $"Mode: {CurrentInputMode}, Input: {movement}, Value: {GetInputValueByMode(movement)}";
    }
}