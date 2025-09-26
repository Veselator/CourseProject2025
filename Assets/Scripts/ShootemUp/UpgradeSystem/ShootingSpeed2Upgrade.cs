
public class ShootingSpeed2Upgrade : IUpgrade
{
    public string MainText => "«б≥льшити швидк≥ть постр≥л≥в";
    public string SecondText => "50%";
    public void ApplyUpgrade()
    {
        PlayerInstances.playerBulletSpawner.ShootingDelay *= 0.5f;
    }
}
