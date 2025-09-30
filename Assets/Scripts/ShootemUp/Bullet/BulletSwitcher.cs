using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class BulletSwitcher : MonoBehaviour
{
    [SerializeField] private InputActionReference[] bulletActions = new InputActionReference[4];
    [SerializeField] private InputActionReference scrollAction; // Добавьте это в Inspector

    private BulletsManagmentSystem bms;
    private BulletSpawner playerBulletSpawner;
    [SerializeField] private BulletType[] bulletTypes = new BulletType[4];

    public BulletType[] AllPlayerBullets => bulletTypes;
    private int currentBulletIndex = 0;
    public Action<int> OnBulletSwitched;

    public static BulletSwitcher Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        bms = BulletsManagmentSystem.Instance;
        playerBulletSpawner = PlayerInstances.playerBulletSpawner;
    }

    private void OnEnable()
    {
        // Регистрация кнопок
        for (int i = 0; i < bulletActions.Length; i++)
        {
            int index = i;
            bulletActions[i].action.performed += _ => SelectBullet(index);
            bulletActions[i].action.Enable();
        }

        // Регистрация скролла
        if (scrollAction != null)
        {
            scrollAction.action.performed += OnScroll;
            scrollAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        foreach (var action in bulletActions)
        {
            action.action.Disable();
        }

        if (scrollAction != null)
        {
            scrollAction.action.performed -= OnScroll;
            scrollAction.action.Disable();
        }
    }

    private void OnScroll(InputAction.CallbackContext context)
    {
        float scrollValue = context.ReadValue<Vector2>().y;

        if (scrollValue > 0)
        {
            // Скролл вверх - следующая пуля
            SwitchToNextAvailableBullet(1);
        }
        else if (scrollValue < 0)
        {
            // Скролл вниз - предыдущая пуля
            SwitchToNextAvailableBullet(-1);
        }
    }

    private void SwitchToNextAvailableBullet(int direction)
    {
        int startIndex = currentBulletIndex;
        int attempts = 0;

        while (attempts < bulletTypes.Length)
        {
            currentBulletIndex = (currentBulletIndex + direction + bulletTypes.Length) % bulletTypes.Length;

            BulletType selectedBulletType = bulletTypes[currentBulletIndex];

            if (bms.IsBulletTypeAvailable(selectedBulletType))
            {
                playerBulletSpawner.CurrentBulletType = selectedBulletType;
                OnBulletSwitched?.Invoke(currentBulletIndex);
                return;
            }

            attempts++;
        }

        // Если не нашли доступных пуль, возвращаемся к исходной
        currentBulletIndex = startIndex;
    }

    private void SelectBullet(int index)
    {
        if (currentBulletIndex == index) return;
        if (index < 0 || index > bulletTypes.Length - 1) return;

        BulletType selectedBulletType = bulletTypes[index];
        if (!bms.IsBulletTypeAvailable(selectedBulletType)) return;

        currentBulletIndex = index;
        playerBulletSpawner.CurrentBulletType = selectedBulletType;
        OnBulletSwitched?.Invoke(currentBulletIndex);
    }
}