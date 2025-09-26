using UnityEngine;

[System.Serializable]
public struct EnemyStreamConfig
{
    [Header("Настройки потока врагов")]

    [Tooltip("Тип врага для спавна")]
    public EnemyType enemyType;

    [Tooltip("Паттерн движения врагов")]
    public MovingPatternType movingPatternType;

    [Tooltip("Начальная точка")]
    [Range(0f, 1f)]
    public float startX;

    [Tooltip("Конечная точка")]
    [Range(0f, 1f)]
    public float endX;

    [Tooltip("Количество врагов в потоке")]
    [Range(1, 50)]
    public int count;

    [Header("Дополнительные настройки")]

    [Tooltip("Задержка между спавном врагов (секунды)")]

    [Range(0.1f, 5f)]
    public float delayBetweenSpawns;

    [Tooltip("Задержка после того, как поток закончился")]
    [Range(0.1f, 5f)]
    public float delayAfterEndOfStream;
}
