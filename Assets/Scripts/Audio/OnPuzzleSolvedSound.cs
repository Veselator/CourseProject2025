using UnityEngine;

public class OnPuzzleSolvedSound : MonoBehaviour
{
    private SignalsManager _signalsManager;
    private GameAudioManager _audioManager;
    [SerializeField] private string _onPuzzleSolvedSFX;

    private void Start()
    {
        _audioManager = GameAudioManager.Instance;
        _signalsManager = GetComponent<SignalsManager>();

        _signalsManager.OnPuzzleSolved += HandlePuzzleSolved;
    }

    private void OnDestroy()
    {
        if (_signalsManager != null)
        {
            _signalsManager.OnPuzzleSolved -= HandlePuzzleSolved;
        }
    }

    private void HandlePuzzleSolved()
    {
        if (_audioManager != null && !string.IsNullOrEmpty(_onPuzzleSolvedSFX))
        {
            _audioManager.PlaySound(_onPuzzleSolvedSFX);
        }
    }
}
