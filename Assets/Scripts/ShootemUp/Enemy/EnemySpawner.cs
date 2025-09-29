using Unity.VisualScripting;
using UnityEngine;
using VContainer;

public class EnemySpawner : MonoBehaviour
{
    private EnemyConfig enemyConfig;
    private BulletConfig bulletConfig;
    // VContainer
    private IObjectResolver resolver;

    // �������� ��� ������ ������

    // ��, ����� ������� ����� �������� ��� �������, ����� �������� �����

    // �� ���� ����� �����������, ��� ������� ��� ����� � ��� �������� ����������
    // ��� ������ �����

    public static EnemySpawner Instance { get; private set; }

    [Inject]
    public void Construct(IObjectResolver resolver, EnemyConfig enemyConfig, BulletConfig bulletConfig)
    {
        this.resolver = resolver;
        this.enemyConfig = enemyConfig;
        this.bulletConfig = bulletConfig;
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    //private void Start()
    //{
    //    // ��� �����
    //    SpawnEnemy(new EnemySpawnData(EnemyType.Regular, MovingPatternType.Curved, 
    //        transform.position, new Vector2(4f, -5f), 0.3f));
    //}

    public void SpawnEnemy(EnemySpawnData spawnData)
    {
        GameObject enemyPrefab = enemyConfig.GetPrefab(spawnData.enemyType);
        if (enemyPrefab == null) return;

        // � ������ ������ �����������
        float imageHeight = enemyPrefab.GetComponent<SpriteRenderer>().bounds.size.y;
        Vector2 spawnPosition = new Vector2(spawnData.spawnPosition.x, spawnData.spawnPosition.y + imageHeight);

        GameObject enemyObject = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        BaseEnemy enemy = enemyObject.GetComponent<BaseEnemy>();
        if (enemyObject.TryGetComponent<BulletSpawner>(out var bulletSpawner))
        {
            bulletSpawner.Init(bulletConfig);
        }

        if (enemy != null)
        {
            // �������� ������� ��� ��������
            IMovingPattern pattern = MovingPatternFactory.CreatePattern(
                spawnData.movingPatternType,
                spawnData.spawnPosition,
                spawnData.targetPosition
            );

            enemy.currentMovingPattern = pattern;
        }
    }
}