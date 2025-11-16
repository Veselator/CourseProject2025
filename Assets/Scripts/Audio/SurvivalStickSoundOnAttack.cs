using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalStickSoundOnAttack : MonoBehaviour
{
    [SerializeField] private Player_Attack _playerAttack;
    private GameAudioManager _audioManager;
    [SerializeField] private SoundWithRandomPitchSettings _attackSoundSettings;

    private void Start()
    {
        _audioManager = GameAudioManager.Instance;
        _playerAttack = Player_Attack.Instance;
        _playerAttack.OnAttack += HandleOnAttack;
    }

    private void OnDestroy()
    {
        if (_playerAttack != null)
        {
            _playerAttack.OnAttack -= HandleOnAttack;
        }
    }
    private void HandleOnAttack()
    {
        _audioManager.PlaySFXWithRandomPitch(_attackSoundSettings);
    }
}
