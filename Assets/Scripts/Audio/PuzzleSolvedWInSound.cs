using UnityEngine;

public class PuzzleSolvedWInSound : MonoBehaviour
{
    private Gm _gm;
    private GameAudioManager _gameAudioManager;
    [SerializeField] private string _winSound;

    private void Start()
    {
        _gm = Gm.Instance;
        _gameAudioManager = GameAudioManager.Instance;

        _gm.OnPuzzleSolved += HandlePuzzleSolved;
    }

    private void OnDestroy()
    {
        if (_gm != null)
        {
            _gm.OnPuzzleSolved -= HandlePuzzleSolved;
        }
    }

    private void HandlePuzzleSolved()
    {
        _gameAudioManager.PlaySound(_winSound);
    }
}
