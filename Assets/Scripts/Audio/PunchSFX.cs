using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchSFX : MonoBehaviour
{
    [SerializeField] private PlayerPunchManager _playerPunchManager;
    [SerializeField] private SoundWithRandomPitchSettings _punchSoundSettings;
    private GameAudioManager _gameAudioManager;

    private void Start()
    {
        _gameAudioManager = GameAudioManager.Instance;
        _playerPunchManager.OnPunch += HandleOnPunch;
    }

    private void OnDestroy()
    {
        _playerPunchManager.OnPunch -= HandleOnPunch;
    }

    private void HandleOnPunch()
    {
        _gameAudioManager.PlaySFXWithRandomPitch(_punchSoundSettings);
    }
}
