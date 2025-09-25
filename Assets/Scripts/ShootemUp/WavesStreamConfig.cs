using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveStreamConfig", menuName = "Shoot`em up/Wave Stream Config")]
// Поток волн
public class WavesStreamConfig : ScriptableObject
{
    [Header("Конфигурация волн уровня")]
    [Tooltip("Массив всех волн в уровне. Выполняются последовательно.")]
    [SerializeField] private WaveConfig[] waves;

    [Tooltip("Время, через которое начнётся волна")]
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
