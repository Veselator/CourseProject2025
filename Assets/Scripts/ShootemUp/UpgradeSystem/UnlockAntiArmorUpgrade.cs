public class UnlockAntiArmorUpgrade : IUpgrade
{
    public string MainText => "����� ��� ����";
    public string SecondText => "����� ����";
    public void ApplyUpgrade()
    {
        BulletsManagmentSystem.UnlockBullet(BulletType.AntiArmor);
    }
}
