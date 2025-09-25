using UnityEngine;

[System.Serializable]
public struct EnemyStreamConfig
{
    [Header("��������� ������ ������")]

    [Tooltip("��� ����� ��� ������")]
    public EnemyType enemyType;

    [Tooltip("������� �������� ������")]
    public MovingPatternType movingPatternType;

    [Tooltip("��������� �����")]
    [Range(0f, 1f)]
    public float startX;

    [Tooltip("�������� �����")]
    [Range(0f, 1f)]
    public float endX;

    [Tooltip("���������� ������ � ������")]
    [Range(1, 50)]
    public int count;

    [Header("�������������� ���������")]

    [Tooltip("�������� ����� ������� ������ (�������)")]

    [Range(0.1f, 5f)]
    public float delayBetweenSpawns;

    [Tooltip("�������� ����� ����, ��� ����� ����������")]
    [Range(0.1f, 5f)]
    public float delayAfterEndOfStream;
}
