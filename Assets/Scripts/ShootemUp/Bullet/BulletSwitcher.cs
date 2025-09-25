using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class BulletSwitcher : MonoBehaviour
{
    [SerializeField] private InputActionReference[] bulletActions = new InputActionReference[4];
    private BulletsManagmentSystem bms;
    private BulletSpawner playerBulletSpawner;
    [SerializeField] private BulletType[] bulletTypes = new BulletType[4]; // Какие типы доступны игроку в принципе

    private int currentBulletIndex = 0;

    private void Start()
    {
        bms = BulletsManagmentSystem.Instance;
        playerBulletSpawner = PlayerInstances.playerBulletSpawner;
    }

    private void OnEnable()
    {
        for (int i = 0; i < bulletActions.Length; i++)
        {
            int index = i; // Захватываем значение для замыкания
            bulletActions[i].action.performed += _ => SelectBullet(index);
            bulletActions[i].action.Enable();
        }
    }

    private void OnDisable()
    {
        foreach (var action in bulletActions)
        {
            action.action.Disable();
        }
    }

    private void SelectBullet(int index)
    {
        if (currentBulletIndex == index) return;
        if (index < 0 || index > bulletTypes.Length - 1) return;
        BulletType selectedBulletType = bulletTypes[index];

        if (!bms.IsBulletTypeAvailable(selectedBulletType)) return;

        // Если мы дошли до сюда - следовательно, всё нормально и мы можем переключать
        // 
        currentBulletIndex = index;
        playerBulletSpawner.CurrentBulletType = selectedBulletType;
    }
}
