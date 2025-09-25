using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    // Общая система ввода для игрока
    // Использует Unity Input System

    private PlayerInputAction _playerInputActions;

    public PlayerInputAction playerInputAction => _playerInputActions;

    public static PlayerInput Instance { get; private set; }

    void Awake()
    {
        _playerInputActions = new PlayerInputAction();
        _playerInputActions.Enable();

        if (Instance == null) Instance = this;
    }

    public Vector2 GetMovementVector()
    {
        return _playerInputActions.Player.Move.ReadValue<Vector2>().normalized;
    }

    public bool IsHitButtonPressed() // Spacebar
    {
        return _playerInputActions.Player.Jump.triggered;
    }
}
