using UnityEngine;

public class QuestItemSounds : MonoBehaviour
{
    [SerializeField] private QuestInventoryManager _inventoryManager;
    private GameAudioManager _audioManager;

    [SerializeField] private SoundWithRandomPitchSettings _itemTook;

    private void Start()
    {
        _audioManager = GameAudioManager.Instance;

        _inventoryManager.OnItemAdded += HandleItemAdded;
    }

    private void OnDestroy()
    {
        _inventoryManager.OnItemAdded -= HandleItemAdded;
    }

    private void HandleItemAdded(QuestInventoryItem item)
    {
        _audioManager.PlaySFXWithRandomPitch(_itemTook);
    }
}