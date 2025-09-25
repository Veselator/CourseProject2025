using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

public class WavesManager : MonoBehaviour
{
    private WavesStreamConfig _waves;
    private EnemySpawner _enemySpawner;

    public int TotalNumOfWaves => _waves.TotalNumOfWaves;
    private int currentWaveIndex;
    public int TotalNumOfEnemiesInWave { get; private set; }

    [SerializeField] private Box spawningArea;
    public static WavesManager Instance { get; private set; }
    public bool IsWaveEnded { get; private set; }

    [Inject]
    public void Construct(WavesStreamConfig wavesStrConfig)
    {
        _waves = wavesStrConfig;
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        currentWaveIndex = 0;
        _enemySpawner = EnemySpawner.Instance;
        GlobalFlags.onFlagChanged += CheckGlobalFlag;

        StartWave();
    }

    private void OnDestroy()
    {
        GlobalFlags.onFlagChanged -= CheckGlobalFlag;
    }

    private void CheckGlobalFlag(string flagName, bool flagState)
    {
        Debug.Log($"Checking global flag {flagName} {flagState}");
        if (flagName == GlobalFlags.Flags.SHOOTEMUP_START_WAVE)
        {
            StartWave();
        }
    }

    public void StartWave()
    {
        Debug.Log($"WAVE {currentWaveIndex} STARTED!");
        StartCoroutine(StartWaveWithDelay());
    }

    private float TranslateNormalisedNumberToPosition(float inputNumber)
    {
        return Mathf.Lerp(spawningArea.startPoint.x, spawningArea.endPoint.x, inputNumber);
    }

    private IEnumerator StartWaveWithDelay()
    {
        yield return new WaitForSeconds(_waves.timeBeforeStart);
        IsWaveEnded = false;

        WaveConfig currentWave = _waves[currentWaveIndex];
        TotalNumOfEnemiesInWave = currentWave.enemyStreamConfigs.Sum(a => a.count);
        Debug.Log($"Total num of enemies in current wave: {TotalNumOfEnemiesInWave}");

        foreach (var enemyStream in currentWave.enemyStreamConfigs)
        {
            // Формируем структуру
            EnemySpawnData enemySpawnData = new EnemySpawnData(
                enemyStream.enemyType,
                enemyStream.movingPatternType,
                new Vector2(TranslateNormalisedNumberToPosition(enemyStream.startX), spawningArea.endPoint.y),
                new Vector2(TranslateNormalisedNumberToPosition(enemyStream.endX), spawningArea.startPoint.y),
                enemyStream.delayBetweenSpawns
            );

            for (int i = 0; i < enemyStream.count; i++)
            {
                _enemySpawner.SpawnEnemy(enemySpawnData);
                yield return new WaitForSeconds(enemyStream.delayBetweenSpawns);
            }

            yield return new WaitForSeconds(enemyStream.delayAfterEndOfStream);
        }
        currentWaveIndex++;
        IsWaveEnded = true;

        // Проверяем, закончились ли волны
        if (currentWaveIndex == TotalNumOfWaves)
        {
            GlobalFlags.ToggleFlag(GlobalFlags.Flags.GAME_WIN);
            yield break;
        }

        // Переключаем только тогда, когда все враги умрут
        //GlobalFlags.ToggleFlag(GlobalFlags.Flags.SHOOTEMUP_WAVE_ENDED);
    }
}
