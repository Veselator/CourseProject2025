using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUIVisual : MonoBehaviour
{
    // Визуальное отображение абилки
    [SerializeField] private AbilityChangerManager _abilityChangerManager;
    [SerializeField] private int _linkedItemID = 0;

    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private ItemVisualUI _highlight;

    private AbilityUIData _linkedAbility;
    private IAbility _currentAbility;
    public string Title => _linkedAbility != null ? _linkedAbility.Title : "NO TITLE";
    public string Description => _linkedAbility != null ? _linkedAbility.Description : "NO DESCRIPTION";

    private void Awake()
    {
        _abilityChangerManager.OnAbilitiesChanged += HoldAbilityChanged;
    }

    private void Start()
    {
        _highlight.Highlight(true);
    }

    private void OnDestroy()
    {
        _abilityChangerManager.OnAbilitiesChanged -= HoldAbilityChanged;
        if (_currentAbility != null) _currentAbility.OnAbilityAvailabilityChanged -= HoldAbilityAvailability;
    }

    private void HoldAbilityChanged(AbilityPanel abilities)
    {
        // Отписываемя от предыдущей способности (если таковая была)
        if(_currentAbility != null)_currentAbility.OnAbilityAvailabilityChanged -= HoldAbilityAvailability;
        _currentAbility = abilities.abilities[_linkedItemID];
        _currentAbility.OnAbilityAvailabilityChanged += HoldAbilityAvailability;
        HoldAbilityAvailability(_currentAbility.IsAvailable);

        _linkedAbility = _currentAbility.UIData;
        _image.sprite = _linkedAbility.AbilitySprite;
        _text.text = _linkedAbility.Title;
    }

    private void HoldAbilityAvailability(bool state)
    {
        _highlight.Highlight(state);
    }
}
