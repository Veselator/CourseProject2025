public class UnlockBouncingUpgrade : IUpgrade
{
    public string MainText => "Новий тип куль";
    public string SecondText => "Відскакуючи";
    public void ApplyUpgrade()
    {
        BulletsManagmentSystem.UnlockBullet(BulletType.Bouncing);
    }
}
