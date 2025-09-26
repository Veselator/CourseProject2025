public class ShootingSpeed1Upgrade : IUpgrade
{
    public string MainText => "«б≥льшити швидк≥ть постр≥л≥в";
    public string SecondText => "30%";
    public void ApplyUpgrade()
    {
        PlayerInstances.playerBulletSpawner.ShootingDelay *= 0.7f;
    }
}
