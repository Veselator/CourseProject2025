using UnityEngine;

public class PickaupableItemSFX : MonoBehaviour
{
    private IPPickupableItem _pickupableItem;
    private GameAudioManager _gameAudioManager;

    [SerializeField] private SoundWithRandomPitchSettings _pickupSoundSettings;

    private void Start()
    {
        _gameAudioManager = GameAudioManager.Instance;
        _pickupableItem = GetComponent<IPPickupableItem>();

        _pickupableItem.OnItemPickedUp += HandleItemPickedUp;
    }

    private void OnDestroy()
    {
        _pickupableItem.OnItemPickedUp -= HandleItemPickedUp;
    }

    private void HandleItemPickedUp()
    {
        _gameAudioManager.PlaySFXWithRandomPitch(_pickupSoundSettings);
    }
}
