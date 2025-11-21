using System;
using UnityEngine;

public class AbilityStrongPunch : IAbility
{
    public AbilityType Type { get; } = AbilityType.StrongPunch;
    private PlayerPunchManager _playerPunchManager;
    public bool IsAvailable { get; set; } = true;
    public AbilityUIData UIData { get; }
    public event Action<bool> OnAbilityAvailabilityChanged;

    public AbilityStrongPunch(PlayerPunchManager ppm, AbilityUIData data)
    {
        _playerPunchManager = ppm;
        UIData = data;
    }

    public bool Try2ApplyAbility()
    {
        _playerPunchManager.Punch();
        return true;
    }
}
