using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAbilityManager : MonoBehaviour
{
    // Мененджер текущий способностей

    public AbilityPanel currentAbilitiesPanel;
    [SerializeField] private InputActionReference[] _inputActions = new InputActionReference[2];

    public event Action<IAbility> OnAbilityApplied;
    private void Start()
    {
        InitInputAction();
    }

    private void InitInputAction()
    {
        _inputActions[0].action.performed += Try2DoFirstAbility;
        _inputActions[0].action.Enable();

        _inputActions[1].action.performed += Try2DoSecondAbility;
        _inputActions[1].action.Enable();
    }

    private void OnDestroy()
    {
        _inputActions[0].action.performed -= Try2DoFirstAbility;
        _inputActions[0].action.Disable();

        _inputActions[1].action.performed -= Try2DoSecondAbility;
        _inputActions[1].action.Disable();
    }

    private void Try2DoFirstAbility(InputAction.CallbackContext context)
    {
        ApplyAbility(0);
    }


    private void Try2DoSecondAbility(InputAction.CallbackContext context)
    {
        ApplyAbility(1);
    }

    private void ApplyAbility(int abilityId)
    {
        IAbility ability = currentAbilitiesPanel.abilities[abilityId];
        if (currentAbilitiesPanel.abilities[abilityId] == null) return;

        if (ability.Try2ApplyAbility()) OnAbilityApplied?.Invoke(ability);
    }
}

// Структура панели с абилками
public struct AbilityPanel
{
    public IAbility[] abilities;

    public AbilityPanel(IAbility firstAbility, IAbility secondAbility)
    {
        abilities = new IAbility[2];
        abilities[0] = firstAbility;
        abilities[1] = secondAbility;
    }
}
