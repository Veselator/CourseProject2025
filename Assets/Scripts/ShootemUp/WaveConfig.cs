using UnityEngine;

[System.Serializable]
public struct WaveConfig
{
    [Header("������ ������ � �����")]
    [Tooltip("������ ������� ������. ����� ���������� ������������.")]
    public EnemyStreamConfig[] enemyStreamConfigs;
}
