using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthRestartEveryWave : MonoBehaviour
{
    private WavesManager wavesManager;
    private Health playerHealth;

    private void Start()
    {
        wavesManager = WavesManager.Instance;
        playerHealth = PlayerInstances.playerHealth;

        wavesManager.OnWaveEnded += ResetHealth;
    }

    private void OnDestroy()
    {
        wavesManager.OnWaveEnded -= ResetHealth;
    }

    private void ResetHealth()
    {
        playerHealth.CurrentHealth = playerHealth.MaximumHealth;
        playerHealth.OnHealthChanged?.Invoke();
    }
}
