using System.Collections.Generic;
using System.Linq;

// Класс для конфигурации апгрейдов
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

    // Распределение апгрейдов по волнам (индексы волн: 0-8 для волн 1-9)
    public static Dictionary<int, HashSet<UpgradeType>> UpgradesOverWaves = new Dictionary<int, HashSet<UpgradeType>>()
    {
        // Волна 1 - базовые апгрейды
        { 0, new HashSet<UpgradeType>
            {
                UpgradeType.HealthUpgrade1,
                UpgradeType.ShootingSpeed1,
                UpgradeType.SpeedUpgrade1,
                UpgradeType.UnlockBouncing
            }
        },
        // Волна 2 - добавляется UnlockAntiArmor
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
        // Волна 3
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
        // Волна 4
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
        // Волна 5 - добавляется UnlockExplosice и UnlockSecondWeapons
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
        // Волны 6-9 - все апгрейды доступны
        { 5, new HashSet<UpgradeType>(upgrades.Keys) },
        { 6, new HashSet<UpgradeType>(upgrades.Keys) },
        { 7, new HashSet<UpgradeType>(upgrades.Keys) },
        { 8, new HashSet<UpgradeType>(upgrades.Keys) }
    };
}