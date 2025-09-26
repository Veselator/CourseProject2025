using UnityEngine;

[System.Serializable]
public struct WaveConfig
{
    [Header("Потоки врагов в волне")]
    [Tooltip("Массив потоков врагов. Могут спавниться одновременно.")]
    public EnemyStreamConfig[] enemyStreamConfigs;
}
