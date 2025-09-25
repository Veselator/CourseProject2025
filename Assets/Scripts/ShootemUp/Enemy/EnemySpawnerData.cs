using UnityEngine;

[System.Serializable]
public struct EnemySpawnData
{
    public EnemyType enemyType;
    public MovingPatternType movingPatternType;
    public Vector2 spawnPosition;
    public Vector2 targetPosition; // Конечная
    public float spawnDelay; // Задержка спавна, будет использоваться в WavesManager

    public EnemySpawnData(EnemyType enemyType, MovingPatternType movingPatternType, Vector2 spawnPosition, Vector2 targetPosition, float spawnDelay)
    {
        this.enemyType = enemyType;
        this.movingPatternType = movingPatternType;
        this.spawnPosition = spawnPosition;
        this.targetPosition = targetPosition;
        this.spawnDelay = spawnDelay;
    }
}