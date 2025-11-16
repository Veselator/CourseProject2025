using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOnHealthDied : MonoBehaviour
{
    private IHealth _health;
    [SerializeField] private SoundWithRandomPitchSettings _deathSoundSettings;

    private void Start()
    {
        _health = GetComponent<IHealth>();
        _health.OnDeath += HandleDeath;
    }

    private void OnDestroy()
    {
        _health.OnDeath -= HandleDeath;
    }

    private void HandleDeath()
    {
        GameAudioManager.Instance.PlaySFXWithRandomPitch(_deathSoundSettings);
    }
}
