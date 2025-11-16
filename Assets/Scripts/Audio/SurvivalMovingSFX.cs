using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalMovingSFX : MonoBehaviour
{
    [SerializeField] private Player_Movement _playerMovement;
    private GameAudioManager _gameAudioManager;

    [SerializeField] private SoundWithRandomPitchSettings _walkingSFXSettings;
    [SerializeField] private SoundWithRandomPitchSettings _runningSFXSettings;

    private void Start()
    {
        _gameAudioManager = GameAudioManager.Instance;

        _playerMovement.OnMovingStarted += HandleMovingStarted;
        _playerMovement.OnMovingEnded += HandleMovingEnded;

        _playerMovement.OnSprintingStarted += HandleSprintingStarted;
        _playerMovement.OnSprintingEnded += HandleSprintingEnded;
    }

    private void HandleSprintingEnded()
    {
        _gameAudioManager.StopLoopingSound(_runningSFXSettings.SoundId);
        if (_playerMovement.IsMoving)
        {
            _gameAudioManager.PlayLoopingSound(_walkingSFXSettings.SoundId);
        }
    }

    private void HandleSprintingStarted()
    {
        _gameAudioManager.PlayLoopingSound(_runningSFXSettings.SoundId);
    }

    private void HandleMovingEnded()
    {
        _gameAudioManager.StopLoopingSound(_walkingSFXSettings.SoundId);
    }

    private void HandleMovingStarted()
    {
        _gameAudioManager.PlayLoopingSound(_walkingSFXSettings.SoundId);
    }
}
