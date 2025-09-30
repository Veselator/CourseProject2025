using System.Collections.Generic;
using System.Linq;

// ����� ��� ������������ ���������
public class UpgradesConfig
{
    public static Dictionary<UpgradeType, IUpgrade> upgrades = new Dictionary<UpgradeType, IUpgrade>()
    {
        { UpgradeType.HealthUpgrade1, new HealthUpgrade1() },
        { UpgradeType.HealthUpgrade2, new HealthUpgrade2() },
        { UpgradeType.UnlockAntiArmor, new UnlockAntiArmorUpgrade() },
        { UpgradeType.UnlockExplosice, new UnlockExplosiceUpgrade() },
        { UpgradeType.UnlockBouncing, new UnlockBouncingUpgrade() },
        { UpgradeType.ShootingSpeed1, new ShootingSpeed1Upgrade() },
        { UpgradeType.ShootingSpeed2, new ShootingSpeed2Upgrade() },
        { UpgradeType.UnlockSecondWeapons, new UnlockSecondWeapons() },
        { UpgradeType.SpeedUpgrade1, new SpeedUpgrade1() },
        { UpgradeType.SpeedUpgrade2, new SpeedUpgrade2() },
    };

    // ������������� ��������� �� ������ (������� ����: 0-8 ��� ���� 1-9)
    public static Dictionary<int, HashSet<UpgradeType>> UpgradesOverWaves = new Dictionary<int, HashSet<UpgradeType>>()
    {
        // ����� 1 - ������� ��������
        { 0, new HashSet<UpgradeType>
            {
                UpgradeType.HealthUpgrade1,
                UpgradeType.ShootingSpeed1,
                UpgradeType.SpeedUpgrade1,
                UpgradeType.UnlockBouncing
            }
        },
        // ����� 2 - ����������� UnlockAntiArmor
        { 1, new HashSet<UpgradeType>
            {
                UpgradeType.HealthUpgrade1,
                UpgradeType.HealthUpgrade2,
                UpgradeType.ShootingSpeed1,
                UpgradeType.SpeedUpgrade1,
                UpgradeType.UnlockBouncing,
                UpgradeType.UnlockAntiArmor
            }
        },
        // ����� 3
        { 2, new HashSet<UpgradeType>
            {
                UpgradeType.HealthUpgrade1,
                UpgradeType.HealthUpgrade2,
                UpgradeType.ShootingSpeed1,
                UpgradeType.ShootingSpeed2,
                UpgradeType.SpeedUpgrade1,
                UpgradeType.SpeedUpgrade2,
                UpgradeType.UnlockBouncing,
                UpgradeType.UnlockAntiArmor
            }
        },
        // ����� 4
        { 3, new HashSet<UpgradeType>
            {
                UpgradeType.HealthUpgrade1,
                UpgradeType.HealthUpgrade2,
                UpgradeType.ShootingSpeed1,
                UpgradeType.ShootingSpeed2,
                UpgradeType.SpeedUpgrade1,
                UpgradeType.SpeedUpgrade2,
                UpgradeType.UnlockBouncing,
                UpgradeType.UnlockAntiArmor
            }
        },
        // ����� 5 - ����������� UnlockExplosice � UnlockSecondWeapons
        { 4, new HashSet<UpgradeType>
            {
                UpgradeType.HealthUpgrade1,
                UpgradeType.HealthUpgrade2,
                UpgradeType.ShootingSpeed1,
                UpgradeType.ShootingSpeed2,
                UpgradeType.SpeedUpgrade1,
                UpgradeType.SpeedUpgrade2,
                UpgradeType.UnlockBouncing,
                UpgradeType.UnlockAntiArmor,
                UpgradeType.UnlockExplosice,
                UpgradeType.UnlockSecondWeapons
            }
        },
        // ����� 6-9 - ��� �������� ��������
        { 5, new HashSet<UpgradeType>(upgrades.Keys) },
        { 6, new HashSet<UpgradeType>(upgrades.Keys) },
        { 7, new HashSet<UpgradeType>(upgrades.Keys) },
        { 8, new HashSet<UpgradeType>(upgrades.Keys) }
    };
}