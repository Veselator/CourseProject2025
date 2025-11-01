using TMPro;
using UnityEngine;

public class GrenadesUIManager : MonoBehaviour
{
    // Отвечает за отображение гранат в UI

    [SerializeField] private GameObject _linkedGameObject;
    [SerializeField] private TMP_Text _linkedText;
    [SerializeField] private GrenadesManager _grenadesManager;
    [SerializeField] private PlayerChangerManager _playerChangerManager;

    private void Awake()
    {
        _playerChangerManager.OnCharacterChanged += HandleCharacterChanged;
        _grenadesManager.OnGrenadeCountChanged += HandleGrenadesCountChanged;

        HandleGrenadesCountChanged(_grenadesManager.CurrentNumOfGrenades);
    }

    private void OnDestroy()
    {
        _playerChangerManager.OnCharacterChanged -= HandleCharacterChanged;
        _grenadesManager.OnGrenadeCountChanged -= HandleGrenadesCountChanged;
    }

    private void HandleGrenadesCountChanged(int newCount)
    {
        _linkedText.text = newCount.ToString();
    }

    private void HandleCharacterChanged(int id)
    {
        _linkedGameObject.SetActive(id == 0);
        if(id == 0)
        {
            HandleGrenadesCountChanged(_grenadesManager.CurrentNumOfGrenades);
        }
    }
}
