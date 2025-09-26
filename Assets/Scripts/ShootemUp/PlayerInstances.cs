using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInstances : MonoBehaviour
{
    // Класс, который обеспечивает глобальный доступ к компонентам игрока
    public static RigidbodyMovement playerRigidbodyMovement { get; private set; }
    public static BulletSpawner playerBulletSpawner { get; private set; }
    public static Health playerHealth { get; private set; }
    public GameObject additionalGuns;

    public static PlayerInstances Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;

        playerRigidbodyMovement = GetComponent<RigidbodyMovement>();
        playerBulletSpawner = GetComponent<BulletSpawner>();
        playerHealth = GetComponent<Health>();
    }
}
