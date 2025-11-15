using UnityEngine;

public class SoundForItemUsedOn : MonoBehaviour
{
    [SerializeField] private QuestActionProccessor _questActionProccessor;
    private GameAudioManager _audioManager;

    [SerializeField] private SoundWithRandomPitchSettings _itemUsed;
    [SerializeField] private string _failed;

    private void Start()
    {
        _audioManager = GameAudioManager.Instance;

        _questActionProccessor.OnItemActionSucceeded += HandleItemActionSucceeded;
        _questActionProccessor.OnItemActionFailed += HandleItemActionFailed;
    }

    private void OnDestroy()
    {
        _questActionProccessor.OnItemActionSucceeded -= HandleItemActionSucceeded;
        _questActionProccessor.OnItemActionFailed -= HandleItemActionFailed;
    }

    private void HandleItemActionSucceeded()
    {
        _audioManager.PlaySFXWithRandomPitch(_itemUsed);
    }

    private void HandleItemActionFailed()
    {
        _audioManager.PlaySound(_failed);
    }
}
