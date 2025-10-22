using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityStrongPunch : IAbility
{
    private PlayerPunchManager _playerPunchManager;

    public AbilityStrongPunch(PlayerPunchManager ppm)
    {
        _playerPunchManager = ppm;
    }

    public void Try2ApplyAbility()
    {
        Debug.Log("Punching!");
        _playerPunchManager.Punch();
    }
}
