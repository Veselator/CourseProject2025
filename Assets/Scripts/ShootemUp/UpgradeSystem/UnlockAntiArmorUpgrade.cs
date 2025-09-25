public class UnlockAntiArmorUpgrade : IUpgrade
{
    public string MainText => "Новий тип куль";
    public string SecondText => "Проти броні";
    public void ApplyUpgrade()
    {
        BulletsManagmentSystem.UnlockBullet(BulletType.AntiArmor);
    }
}
