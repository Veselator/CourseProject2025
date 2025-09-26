using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockExplosiceUpgrade : IUpgrade
{
    public string MainText => "����� ��� ����";
    public string SecondText => "������";
    public void ApplyUpgrade()
    {
        BulletsManagmentSystem.UnlockBullet(BulletType.Explosive);
    }
}
