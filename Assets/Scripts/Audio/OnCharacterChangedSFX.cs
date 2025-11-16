using UnityEngine;

public class OnCharacterChangedSFX : MonoBehaviour
{
    [SerializeField] private PlayerChangerManager _playerChangerManager;
    private GameAudioManager _audioManager;
    [SerializeField] private string _onPlayerChangedSFX;

    private void Start()
    {
        _audioManager = GameAudioManager.Instance;

        _playerChangerManager.OnCharacterChanged += HandlePuzzleSolved;
    }

    private void OnDestroy()
    {
        if (_playerChangerManager != null)
        {
            _playerChangerManager.OnCharacterChanged -= HandlePuzzleSolved;
        }
    }

    private void HandlePuzzleSolved(int _)
    {
        _audioManager.PlaySound(_onPlayerChangedSFX);
    }
}
