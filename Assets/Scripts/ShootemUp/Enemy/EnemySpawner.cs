using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyConfig enemyConfig;

    // Вызываем для спавна врагов

    // То, какой паттерн будет применим для объекта, будет решаться извне

    // То есть имеем архитектуру, при которой тип врага и тип паттерна независимы
    // Что просто супер

    public static EnemySpawner Instance { get; private set; }

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

        GameObject enemyObject = Instantiate(enemyPrefab, spawnData.spawnPosition, Quaternion.identity);

        BaseEnemy enemy = enemyObject.GetComponent<BaseEnemy>();
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