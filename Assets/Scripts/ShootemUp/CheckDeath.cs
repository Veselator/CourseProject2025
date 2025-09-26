using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckDeath : MonoBehaviour
{
    [SerializeField] private Health trackingHaelth;

    private void Start()
    {
        trackingHaelth.OnDeath += ToggleDeath;
    }

    private void OnDestroy()
    {
        trackingHaelth.OnDeath -= ToggleDeath;
    }

    private void ToggleDeath()
    {
        GlobalFlags.ToggleFlag(GlobalFlags.Flags.GAME_OVER);
    }
}
