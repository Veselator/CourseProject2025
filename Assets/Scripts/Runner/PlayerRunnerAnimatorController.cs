using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.StandaloneInputModule;

public class PlayerRunnerAnimatorController : MonoBehaviour
{
    [SerializeField] private Animator _playerAnimator;
    private PlayerInputHandler _playerInputHandler;
    private PlayerHealth _playerHealth;

    private void Start()
    {
        _playerInputHandler = PlayerInputHandler.Instance;
        _playerHealth = PlayerHealth.Instance;

        _playerInputHandler.OnInputModeChanged += OnInputModeChanged;
        _playerHealth.OnPlayerDied += PlayerDied;
    }

    private void OnDestroy()
    {
        _playerInputHandler.OnInputModeChanged -= OnInputModeChanged;
        _playerHealth.OnPlayerDied -= PlayerDied;
    }

    private void OnInputModeChanged(InputMode inputMode)
    {
        Debug.Log($" I just changed input mode to {inputMode} LOL");
        _playerAnimator.SetBool("IsHorizontalRoad", inputMode == InputMode.Vertical);
    }

    private void PlayerDied()
    {
        Debug.Log("Player just died LOL");
        _playerAnimator.StopPlayback();
    }
}
