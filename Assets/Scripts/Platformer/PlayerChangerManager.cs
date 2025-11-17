using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerChangerManager : MonoBehaviour
{
    // Логика смены персонажа; выбор из двух

    private int currentCharacter = 0; // Alex - 0, Borys - 1
    public int CurrentCharacter => currentCharacter;
    [SerializeField] private InputActionReference inputAction2Change;

    public static PlayerChangerManager Instance;

    public event Action<int> OnCharacterChanged;

    private void Awake()
    {
        Instance = this;
        BindChanging();
    }

    private void BindChanging()
    {
        inputAction2Change.action.performed += ChangeCharacter;
        inputAction2Change.action.Enable();
    }

    private void OnDestroy()
    {
        inputAction2Change.action.performed -= ChangeCharacter;
        inputAction2Change.action.Disable();
    }

    // Перегрузка для inputAction
    private void ChangeCharacter(InputAction.CallbackContext context)
    {
        ChangeCharacter();
    }

    public void ChangeCharacter()
    {
        currentCharacter = 1 - currentCharacter;
        OnCharacterChanged?.Invoke(currentCharacter);
    }
}
