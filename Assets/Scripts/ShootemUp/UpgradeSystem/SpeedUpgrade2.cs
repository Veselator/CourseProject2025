public class SpeedUpgrade2 : IUpgrade
{
    public string MainText => "�������� �������";
    public string SecondText => "30%";
    public void ApplyUpgrade()
    {
        PlayerInstances.playerBulletSpawner.ShootingDelay *= 1.3f;
    }
}