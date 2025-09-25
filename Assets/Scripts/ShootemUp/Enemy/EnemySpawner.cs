using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyConfig enemyConfig;

    // �������� ��� ������ ������

    // ��, ����� ������� ����� �������� ��� �������, ����� �������� �����

    // �� ���� ����� �����������, ��� ������� ��� ����� � ��� �������� ����������
    // ��� ������ �����

    public static EnemySpawner Instance { get; private set; }

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

        GameObject enemyObject = Instantiate(enemyPrefab, spawnData.spawnPosition, Quaternion.identity);

        BaseEnemy enemy = enemyObject.GetComponent<BaseEnemy>();
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