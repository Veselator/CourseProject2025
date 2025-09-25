using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesManager : MonoBehaviour
{
    private WavesManager _wavesManager;
    [SerializeField] private IUpgrade[] _availableUpgrades;

    public Action OnUpgradesGenerated; // When ready to show ui
    private IUpgrade[] _choosenUpgrades = new IUpgrade[2];

    private void Start()
    {
        _wavesManager = WavesManager.Instance;

        _wavesManager.OnWaveEnded += OnWaveEnded;
    }

    private void OnDestroy()
    {
        _wavesManager.OnWaveEnded -= OnWaveEnded;
    }

    public void ChooseUpgrade(int upgrade)
    {
        // applying upgrade
        _choosenUpgrades[upgrade].ApplyUpgrade();
        GlobalFlags.ToggleFlag(GlobalFlags.Flags.SHOOTEMUP_START_WAVE);
    }

    private void OnWaveEnded()
    {
        // Get upgrades
        // And call action to show ui

        GetUpgrades();
        OnUpgradesGenerated?.Invoke();
    }

    private void GetUpgrades()
    {

    }
}
