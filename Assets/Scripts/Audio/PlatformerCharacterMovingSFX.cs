using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerCharacterMovingSFX : MonoBehaviour
{
    [SerializeField] private PlayerPlatformerHandler _playerPlatformerHandler;
    private GameAudioManager _gameAudioManager;

    [SerializeField] private string _walkingSFX;
    [SerializeField] private SoundWithRandomPitchSettings _jumpingSFX;

    private void Start()
    {
        _gameAudioManager = GameAudioManager.Instance;

        _playerPlatformerHandler.OnPlayerWalking += HandlePlayerWalking;
        _playerPlatformerHandler.OnPlayerDoesntWalking += HandlePlayerDoesntWalking;
        _playerPlatformerHandler.OnPlayerJumped += HandlePlayerJumped;

        _playerPlatformerHandler.OnPlayerDegrounded += HandlePlayerDoesntWalking;
        _playerPlatformerHandler.OnPlayerGrounded += HandlePlayerGrounded;
    }

    private void OnDestroy()
    {
        if (_playerPlatformerHandler != null)
        {
            _playerPlatformerHandler.OnPlayerWalking -= HandlePlayerWalking;
            _playerPlatformerHandler.OnPlayerDoesntWalking -= HandlePlayerDoesntWalking;
            _playerPlatformerHandler.OnPlayerJumped -= HandlePlayerJumped;

            _playerPlatformerHandler.OnPlayerDegrounded -= HandlePlayerDoesntWalking;
            _playerPlatformerHandler.OnPlayerGrounded -= HandlePlayerGrounded;
        }
    }

    private void HandlePlayerWalking()
    {
        _gameAudioManager.PlayLoopingSound(_walkingSFX);
    }

    private void HandlePlayerDoesntWalking()
    {
        _gameAudioManager.StopLoopingSound(_walkingSFX);
    }

    private void HandlePlayerGrounded()
    {
        if(_playerPlatformerHandler.MovementVector != Vector2.zero) _gameAudioManager.PlayLoopingSound(_walkingSFX);
    }

    private void HandlePlayerJumped(float jumpForce)
    {
        _gameAudioManager.PlaySFXWithRandomPitch(_jumpingSFX);
    }
}
