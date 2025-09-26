using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockExplosiceUpgrade : IUpgrade
{
    public string MainText => "Новий тип куль";
    public string SecondText => "Взривні";
    public void ApplyUpgrade()
    {
        BulletsManagmentSystem.UnlockBullet(BulletType.Explosive);
    }
}
