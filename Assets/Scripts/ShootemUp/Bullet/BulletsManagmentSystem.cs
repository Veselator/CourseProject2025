using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsManagmentSystem : MonoBehaviour
{
    [SerializeField] private HashSet<BulletType> unlockedBullets = new HashSet<BulletType>();
    public static BulletsManagmentSystem Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    //  ласс дл€ того, что-бы управл€ть доступными пул€ми
    public static void UnlockBullet(BulletType bulletType)
    {
        if (!Instance.unlockedBullets.Contains(bulletType))
        {
            Instance.unlockedBullets.Add(bulletType);
        }
    }

    public bool IsBulletTypeAvailable(BulletType bulletType)
    {
        return unlockedBullets.Contains(bulletType);
    }
}
