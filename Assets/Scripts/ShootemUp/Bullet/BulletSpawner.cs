using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class BulletSpawner : MonoBehaviour
{
    public Vector2 direction;

    [Tooltip("�������� ����� ������� ����")]
    [Min(0.01f)] // ����������� ��������
    [SerializeField] private float delay;
    public bool IsAbleToSpawn = true;

    public float ShootingDelay
    {
        get => delay;
        set
        {
            delay = math.max(value, 0.01f);
        }
    }

    private BulletConfig _bulletConfig;
    [SerializeField] private BulletType currentBulletType;
    public BulletType CurrentBulletType
    {
        get => currentBulletType;
        set
        {
            currentBulletType = value;
            InitBulletGameObject();
        }
    }

    private GameObject currentBulletPrefab;
    [SerializeField] private Transform spawnBulletsPoint;
    [SerializeField] private bool isPlayerSpawner;
    [SerializeField] private float damageMultiplier = 1f;
    public float DamageMultiplayer
    {
        get => damageMultiplier;
        set
        {
            damageMultiplier = Mathf.Max(0f, value);
        }
    }

    private float timer;

    [Inject]
    public void Construct(BulletConfig bulletConfig)
    {
        Init(bulletConfig);
    }

    public void Init(BulletConfig bulletConfig)
    {
        Debug.Log("BulletConfig inited succesfully");
        _bulletConfig = bulletConfig;
    }

    private void OnEnable()
    {
        //if(!isPlayerSpawner && _bulletConfig == null) Init(PlayerInstances.playerBulletSpawner._bulletConfig);
    }

    private void Start()
    {
        timer = 0f;
        InitBulletGameObject();
    }

    private void FixedUpdate()
    {
        if (GlobalFlags.GetFlag(GlobalFlags.Flags.GAME_OVER)) return;
        if (!IsAbleToSpawn) return;
        timer += Time.fixedDeltaTime;
        if (timer >= delay)
        {
            timer = 0f;
            SpawnBullet();
        }
    }

    private void InitBulletGameObject()
    {
        // �������� ���������� � LifetimeScope, ���� ��� ������� ����
        if (_bulletConfig == null) _bulletConfig = LifetimeScope.Find<ShootemUpLifetimeScope>().Container.Resolve<BulletConfig>();
        currentBulletPrefab = _bulletConfig.GetPrefab(currentBulletType);
    }

    private void SpawnBullet()
    {
        GameObject lastBullet = Instantiate(currentBulletPrefab, spawnBulletsPoint.position, Quaternion.identity);
        
        lastBullet.GetComponent<IBullet>().Initialize(direction, isPlayerSpawner, DamageMultiplayer);
        //Debug.Log(lastBullet.GetComponent<IBullet>());
    }
}
