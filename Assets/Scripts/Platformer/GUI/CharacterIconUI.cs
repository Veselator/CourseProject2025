using UnityEngine;
using UnityEngine.UI;

public class CharacterIconUI : MonoBehaviour
{
    [SerializeField] private Sprite[] characterIcons;
    [SerializeField] private Image _linkedImage;
    [SerializeField] private PlayerChangerManager _playerChangerManager;

    private void Start()
    {
        _playerChangerManager.OnCharacterChanged += HoldCharacterChange;
    }

    private void HoldCharacterChange(int newCharacterID)
    {
        if (newCharacterID < 0 || newCharacterID >= characterIcons.Length) return;
        _linkedImage.sprite = characterIcons[newCharacterID];
    }
}
