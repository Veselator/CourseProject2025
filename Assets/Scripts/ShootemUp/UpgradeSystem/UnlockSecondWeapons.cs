using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockSecondWeapons : IUpgrade
{
    public string MainText => "Відкрити";
    public string SecondText => "Другу зброю";
    public void ApplyUpgrade()
    {
        PlayerInstances.Instance.additionalGuns.SetActive(true);
        // Возможно, потребуется как-то графически это передать
        // Изменением спрайта игрока
    }
}
