using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsManagmentSystem : MonoBehaviour
{
    [SerializeField] private HashSet<BulletType> unlockedBullets = new HashSet<BulletType>();
    public int CountOfUnlockedBullets => unlockedBullets.Count;
    public Action<BulletType> OnNewBulletUnlocked;
    public static BulletsManagmentSystem Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UnlockBullet(BulletType.Regular);
    }

    //  ласс дл€ того, что-бы управл€ть доступными пул€ми
    public static void UnlockBullet(BulletType bulletType)
    {
        if (!Instance.unlockedBullets.Contains(bulletType))
        {
            Instance.unlockedBullets.Add(bulletType);
            Instance.OnNewBulletUnlocked?.Invoke(bulletType);
        }
    }

    public bool IsBulletTypeAvailable(BulletType bulletType)
    {
        return unlockedBullets.Contains(bulletType);
    }
}
