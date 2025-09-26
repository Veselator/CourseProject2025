using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveStreamConfig", menuName = "Shoot`em up/Wave Stream Config")]
// ����� ����
public class WavesStreamConfig : ScriptableObject
{
    [Header("������������ ���� ������")]
    [Tooltip("������ ���� ���� � ������. ����������� ���������������.")]
    [SerializeField] private WaveConfig[] waves;

    [Tooltip("�����, ����� ������� ������� �����")]
    [Min(0.1f)]
    public float timeBeforeStart;

    public int TotalNumOfWaves => waves.Length;

    public WaveConfig this[int index]
    {
        get
        {
            if (index < 0 || index >= waves.Count()) return new WaveConfig();
            return waves[index];
        }
    }
}
