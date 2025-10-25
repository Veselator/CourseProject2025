using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityChangerManager : MonoBehaviour
{
    private AbilityPanel[] abilityPanels;
    private PlayerChangerManager _changerManager;
    [SerializeField] private PlayerAbilityManager _playerAbilityManager;
    [SerializeField] private PlayerPunchManager _playerPunchManager;
    [SerializeField] private GrenadesManager _grenadesManager;
    [SerializeField] private LaserTurnOffer _laserTurnOffer;

    private void Start()
    {
        InitAbilityPanels();
        _changerManager = PlayerChangerManager.Instance;
        _changerManager.OnCharacterChanged += ChangeAbility;

        ChangeAbility(_changerManager.CurrentCharacter);
    }

    private void OnDestroy()
    {
        _changerManager.OnCharacterChanged -= ChangeAbility;
    }

    private void InitAbilityPanels()
    {
        abilityPanels = new AbilityPanel[2] {
            new(new AbilityStrongPunch(_playerPunchManager), new AbilityMechanic(_grenadesManager)),
            new(new AbilitySolvePuzzles(), new AbilityTurnOffLazers(_laserTurnOffer))
        };
    }

    private void ChangeAbility(int newAbility)
    {
        _playerAbilityManager.currentAbilitiesPanel = abilityPanels[newAbility];
    }
}
