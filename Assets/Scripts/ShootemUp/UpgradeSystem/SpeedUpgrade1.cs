public class SpeedUpgrade1 : IUpgrade
{
    public string MainText => "�������� �������";
    public string SecondText => "20%";
    public void ApplyUpgrade()
    {
        PlayerInstances.playerBulletSpawner.ShootingDelay *= 1.2f;
    }
}
