using Unity.VisualScripting;
using UnityEngine;
using VContainer;

public class EnemySpawner : MonoBehaviour
{
    private EnemyConfig enemyConfig;
    private BulletConfig bulletConfig;
    // VContainer
    private IObjectResolver resolver;

    // Вызываем для спавна врагов

    // То, какой паттерн будет применим для объекта, будет решаться извне

    // То есть имеем архитектуру, при которой тип врага и тип паттерна независимы
    // Что просто супер

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
    //    // Для теста
    //    SpawnEnemy(new EnemySpawnData(EnemyType.Regular, MovingPatternType.Curved, 
    //        transform.position, new Vector2(4f, -5f), 0.3f));
    //}

    public void SpawnEnemy(EnemySpawnData spawnData)
    {
        GameObject enemyPrefab = enemyConfig.GetPrefab(spawnData.enemyType);
        if (enemyPrefab == null) return;

        // С учётом высоты изображения
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
            // Вызываем фабрику для паттерна
            IMovingPattern pattern = MovingPatternFactory.CreatePattern(
                spawnData.movingPatternType,
                spawnData.spawnPosition,
                spawnData.targetPosition
            );

            enemy.currentMovingPattern = pattern;
        }
    }
}