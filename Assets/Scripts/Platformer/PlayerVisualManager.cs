using UnityEngine;

public class PlayerVisualManager : MonoBehaviour
{
    // Логика смены визуала

    private PlayerChangerManager _changerManager;
    [SerializeField] private GameObject[] _playerVisuals; // 0 - Alex, 1 - Borys

    private void Start()
    {
        _changerManager = PlayerChangerManager.Instance;
        _changerManager.OnCharacterChanged += ChangeVisual;

        ChangeVisual(_changerManager.CurrentCharacter);
    }

    private void OnDestroy()
    {
        _changerManager.OnCharacterChanged -= ChangeVisual;
    }

    private void ChangeVisual(int newCharacter)
    {
        _playerVisuals[1 - newCharacter].SetActive(false);
        _playerVisuals[newCharacter].SetActive(true);
    }
}
