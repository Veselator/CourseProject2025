
public class ShootingSpeed2Upgrade : IUpgrade
{
    public string MainText => "�������� ������� �������";
    public string SecondText => "50%";
    public void ApplyUpgrade()
    {
        PlayerInstances.playerBulletSpawner.ShootingDelay *= 0.5f;
    }
}
