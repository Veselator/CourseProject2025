public class UnlockBouncingUpgrade : IUpgrade
{
    public string MainText => "����� ��� ����";
    public string SecondText => "³���������";
    public void ApplyUpgrade()
    {
        BulletsManagmentSystem.UnlockBullet(BulletType.Bouncing);
    }
}
