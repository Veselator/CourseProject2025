using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePiecePlacedSFX : MonoBehaviour
{
    [SerializeField] private PuzzlePiece _puzzlePiece;
    [SerializeField] private SoundWithRandomPitchSettings _puzzlePiecePlacedSFXSettings;
    private GameAudioManager _gameAudioManager;

    private void Start()
    {
        _gameAudioManager = GameAudioManager.Instance;
        _puzzlePiece.OnPuzzlePiecePlaced += PlayPuzzlePiecePlacedSFX;
    }

    private void OnDestroy()
    {
        _puzzlePiece.OnPuzzlePiecePlaced -= PlayPuzzlePiecePlacedSFX;
    }

    private void PlayPuzzlePiecePlacedSFX()
    {
        _gameAudioManager.PlaySFXWithRandomPitch(_puzzlePiecePlacedSFXSettings);
    }
}
