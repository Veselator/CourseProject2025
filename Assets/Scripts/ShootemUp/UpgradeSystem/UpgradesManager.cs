using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum UpgradeType
{
    // Увеличение скорости
    ShootingSpeed1,
    ShootingSpeed2,

    // Пули
    UnlockAntiArmor,
    UnlockExplosice,
    UnlockBouncing,

    // Здоровье
    HealthUpgrade1,
    HealthUpgrade2,

    // Доп оружие
    UnlockSecondWeapons,

    // Скорость
    SpeedUpgrade1,
    SpeedUpgrade2,
}

public class UpgradesManager : MonoBehaviour
{
    private WavesManager _wavesManager;
    [SerializeField] private List<IUpgrade> _availableUpgrades;

    public Action OnUpgradesReady; // When ready to show ui
    public IUpgrade[] choosenUpgrades = new IUpgrade[2];

    public static UpgradesManager Instance { get; private set; }
    private bool isAvailableToChoseUpgrade;

    private void Awake()
    {
        if(Instance == null) Instance = this;
    }

    private void Start()
    {
        _wavesManager = WavesManager.Instance;
        _wavesManager.OnWaveEnded += OnWaveEnded;

        _availableUpgrades = UpgradesConfig.upgrades.Values.ToList();
    }

    private void OnDestroy()
    {
        _wavesManager.OnWaveEnded -= OnWaveEnded;
    }

    public void ChooseUpgrade(int upgrade)
    {
        if (!isAvailableToChoseUpgrade) return;

        // applying upgrade
        choosenUpgrades[upgrade].ApplyUpgrade();
        isAvailableToChoseUpgrade = false;

        GlobalFlags.ToggleFlag(GlobalFlags.Flags.SHOOTEMUP_START_WAVE);
    }

    private void OnWaveEnded()
    {
        // Get upgrades
        // And call action to show ui

        Debug.Log($"OnWaveEnded called! Time: {Time.time}");
        GetUpgrades();
        isAvailableToChoseUpgrade = true;
        OnUpgradesReady?.Invoke();
    }

    private void GetUpgrades()
    {
        // Получаем два новых апгрейда
        choosenUpgrades[0] = GetRandomListElement(_availableUpgrades);
        choosenUpgrades[1] = GetRandomListElement(_availableUpgrades);
    }

    public static IUpgrade GetRandomListElement(List<IUpgrade> list)
    {
        if (list == null || list.Count == 0)
            return null;

        int randomIndex = UnityEngine.Random.Range(0, list.Count);
        IUpgrade randomElement = list[randomIndex];
        list.RemoveAt(randomIndex);
        return randomElement;
    }
}
