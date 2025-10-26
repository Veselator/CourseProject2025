using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityChangerManager : MonoBehaviour
{
    private AbilityPanel[] abilityPanels;
    private PlayerChangerManager _changerManager;
    [SerializeField] private AbilityUIData[] _abilitiesUIData;
    [SerializeField] private PlayerAbilityManager _playerAbilityManager;
    [SerializeField] private PlayerPunchManager _playerPunchManager;
    [SerializeField] private GrenadesManager _grenadesManager;
    [SerializeField] private LaserTurnOffer _laserTurnOffer;

    public event Action<AbilityPanel> OnAbilitiesChanged;
    private void Start()
    {
        InitAbilityPanels();
        _changerManager = PlayerChangerManager.Instance;
        _changerManager.OnCharacterChanged += ChangeAbility;

        ChangeAbility(_changerManager.CurrentCharacter);
    }

    private void InitAbilityPanels()
    {
        abilityPanels = new AbilityPanel[2] {
            new(new AbilityStrongPunch(_playerPunchManager, _abilitiesUIData[0]), new AbilityMechanic(_grenadesManager, _abilitiesUIData[1])),
            new(new AbilitySolvePuzzles(_abilitiesUIData[2]), new AbilityTurnOffLazers(_laserTurnOffer, _abilitiesUIData[3]))
        };

        AbilityMechanic mech = abilityPanels[0].abilities[1] as AbilityMechanic;
        _grenadesManager.OnGrenadeCountChanged += mech.CheckAreGrenadesAvailable;
    }

    private void OnDestroy()
    {
        _changerManager.OnCharacterChanged -= ChangeAbility;

        AbilityMechanic mech = abilityPanels[0].abilities[1] as AbilityMechanic;
        _grenadesManager.OnGrenadeCountChanged -= mech.CheckAreGrenadesAvailable;
    }

    private void ChangeAbility(int newAbility)
    {
        _playerAbilityManager.currentAbilitiesPanel = abilityPanels[newAbility];
        OnAbilitiesChanged?.Invoke(abilityPanels[newAbility]);
    }
}
