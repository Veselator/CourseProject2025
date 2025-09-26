public class SpeedUpgrade1 : IUpgrade
{
    public string MainText => "«б≥льшити швидк≥ть";
    public string SecondText => "20%";
    public void ApplyUpgrade()
    {
        PlayerInstances.playerBulletSpawner.ShootingDelay *= 1.2f;
    }
}
