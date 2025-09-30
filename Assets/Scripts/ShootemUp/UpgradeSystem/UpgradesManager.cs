using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum UpgradeType
{
    // ���������� ��������
    ShootingSpeed1,
    ShootingSpeed2,
    // ����
    UnlockAntiArmor,
    UnlockExplosice,
    UnlockBouncing,
    // ��������
    HealthUpgrade1,
    HealthUpgrade2,
    // ��� ������
    UnlockSecondWeapons,
    // ��������
    SpeedUpgrade1,
    SpeedUpgrade2,
}

public class UpgradesManager : MonoBehaviour
{
    private WavesManager _wavesManager;
    private HashSet<UpgradeType> _remainingUpgrades; // ���������� ��������

    public Action OnUpgradesReady; // When ready to show ui
    public IUpgrade[] choosenUpgrades = new IUpgrade[2];
    public static UpgradesManager Instance { get; private set; }
    private bool isAvailableToChoseUpgrade;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        _wavesManager = WavesManager.Instance;
        _wavesManager.OnWaveEnded += OnWaveEnded;

        // �������������� ��� ��������� ��������
        _remainingUpgrades = new HashSet<UpgradeType>(UpgradesConfig.upgrades.Keys);
    }

    private void OnDestroy()
    {
        _wavesManager.OnWaveEnded -= OnWaveEnded;
    }

    public void ChooseUpgrade(int upgrade)
    {
        if (!isAvailableToChoseUpgrade) return;
        isAvailableToChoseUpgrade = false;

        // ��������� �������
        IUpgrade selectedUpgrade = choosenUpgrades[upgrade];
        selectedUpgrade.ApplyUpgrade();

        // ������� �������������� ������� �� ����������
        UpgradeType selectedType = GetUpgradeType(selectedUpgrade);
        _remainingUpgrades.Remove(selectedType);

        GlobalFlags.ToggleFlag(GlobalFlags.Flags.SHOOTEMUP_START_WAVE);
    }

    private void OnWaveEnded()
    {
        Debug.Log($"OnWaveEnded called! Time: {Time.time}");

        // �������� ��������
        bool upgradesAvailable = GetUpgrades();

        if (upgradesAvailable)
        {
            isAvailableToChoseUpgrade = true;
            OnUpgradesReady?.Invoke();
        }
        else
        {
            // ���� ��������� �� ��������, ����� �������� ����� �����
            Debug.Log("No upgrades available, starting next wave immediately");
            GlobalFlags.ToggleFlag(GlobalFlags.Flags.SHOOTEMUP_START_WAVE);
        }
    }

    private bool GetUpgrades()
    {
        int currentWave = _wavesManager.CurrentWaveIndex; // �����������, ��� ���� ����� ����

        // �������� ��������, ��������� ��� ������� �����
        if (!UpgradesConfig.UpgradesOverWaves.TryGetValue(currentWave, out HashSet<UpgradeType> waveUpgrades))
        {
            Debug.LogWarning($"No upgrade configuration for wave {currentWave}");
            return false;
        }

        // ����������� ��������� ��� ����� � ���������� ���������
        List<UpgradeType> availableUpgradeTypes = waveUpgrades.Intersect(_remainingUpgrades).ToList();

        // ���������, ���� �� ��������� ��������
        if (availableUpgradeTypes.Count == 0)
        {
            Debug.Log("No upgrades left for this wave");
            return false;
        }

        // ���� ������� ������ ���� �������, �������� ��� ������ (��� ����� ����� ���������)
        if (availableUpgradeTypes.Count == 1)
        {
            UpgradeType singleType = availableUpgradeTypes[0];
            choosenUpgrades[0] = UpgradesConfig.upgrades[singleType];
            choosenUpgrades[1] = UpgradesConfig.upgrades[singleType];
            Debug.Log("Only one upgrade available, showing it in both slots");
            return true;
        }

        // �������� ��� ��������� ��������
        int firstIndex = UnityEngine.Random.Range(0, availableUpgradeTypes.Count);
        UpgradeType firstType = availableUpgradeTypes[firstIndex];
        choosenUpgrades[0] = UpgradesConfig.upgrades[firstType];

        // ������� ������ ��������� ������� �� ������ ��� ������� ������
        availableUpgradeTypes.RemoveAt(firstIndex);

        int secondIndex = UnityEngine.Random.Range(0, availableUpgradeTypes.Count);
        UpgradeType secondType = availableUpgradeTypes[secondIndex];
        choosenUpgrades[1] = UpgradesConfig.upgrades[secondType];

        return true;
    }

    // ��������������� ����� ��� ��������� ���� �������� �� ����������
    private UpgradeType GetUpgradeType(IUpgrade upgrade)
    {
        foreach (var kvp in UpgradesConfig.upgrades)
        {
            if (kvp.Value.GetType() == upgrade.GetType())
            {
                return kvp.Key;
            }
        }
        return default(UpgradeType);
    }
}