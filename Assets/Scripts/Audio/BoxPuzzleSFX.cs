using System;
using UnityEngine;

public class BoxPuzzleSFX : MonoBehaviour
{
    [SerializeField] private BoxPuzzleGameManager _gameManager;
    private GameAudioManager _audioManager;
    private BlockSelectionManager _blockSelectionManager;

    [SerializeField] private SoundWithRandomPitchSettings _successSound;
    [SerializeField] private SoundWithRandomPitchSettings _blockSelectedSound;
    [SerializeField] private SoundWithRandomPitchSettings _blocksConnectedSound;
    [SerializeField] private string _wrongSelectedSound;
    [SerializeField] private string _levelCompeteSound;

    private void Start()
    {
        _audioManager = GameAudioManager.Instance;
        _blockSelectionManager = BlockSelectionManager.Instance;

        _gameManager.OnLevelCompleted += HandleLevelCompleted;
        _blockSelectionManager.OnSuccessAnimation += HandleRightSelected;
        _blockSelectionManager.OnFailureAnimation += HandleFailAnimation;
        _blockSelectionManager.OnBlockSelected += HandleSelected;
        _blockSelectionManager.OnBlocksConnected += HandleBlocksConnected;
    }

    private void OnDestroy()
    {
        _gameManager.OnLevelCompleted -= HandleLevelCompleted;
        _blockSelectionManager.OnSuccessAnimation -= HandleRightSelected;
        _blockSelectionManager.OnFailureAnimation -= HandleFailAnimation;
        _blockSelectionManager.OnBlockSelected -= HandleSelected;
        _blockSelectionManager.OnBlocksConnected -= HandleBlocksConnected;
    }

    private void HandleBlocksConnected()
    {
        _audioManager.PlaySFXWithRandomPitch(_blocksConnectedSound);
    }

    private void HandleSelected(BoxPiece _)
    {
        _audioManager.PlaySFXWithRandomPitch(_blockSelectedSound);
    }

    private void HandleFailAnimation()
    {
        _audioManager.PlaySound(_wrongSelectedSound);
    }

    private void HandleRightSelected()
    {
        _audioManager.PlaySFXWithRandomPitch(_successSound);
    }

    private void HandleLevelCompleted()
    {
        _audioManager.PlaySound(_levelCompeteSound);
    }
}
