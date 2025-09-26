using System.Collections.Generic;

// Класс для кофигурации апгрейдов
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
}