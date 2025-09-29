using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockSecondWeapons : IUpgrade
{
    public string MainText => "Відкрити";
    public string SecondText => "Другу зброю";
    public void ApplyUpgrade()
    {
        foreach(var bulletSpawner in PlayerInstances.Instance.additionalGuns.GetComponents<BulletSpawner>())
        {
            bulletSpawner.enabled = true;
        }
        // Возможно, потребуется как-то графически это передать
        // Изменением спрайта игрока
    }
}
