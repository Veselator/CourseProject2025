using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalOnDamageSound : MonoBehaviour
{
    [SerializeField] private Player_Gets_Damage _linkedDamageTracker;
    private GameAudioManager _gameAudioManager;
    [SerializeField] private SoundWithRandomPitchSettings _onDamageSoundSettings;

    private void Start()
    {
        _gameAudioManager = GameAudioManager.Instance;
        _linkedDamageTracker.OnPlayerDamage += PlayOnDamageSound;
    }

    private void OnDestroy()
    {
        _linkedDamageTracker.OnPlayerDamage -= PlayOnDamageSound;
    }

    private void PlayOnDamageSound()
    {
        _gameAudioManager.PlaySFXWithRandomPitch(_onDamageSoundSettings);
    }
}
