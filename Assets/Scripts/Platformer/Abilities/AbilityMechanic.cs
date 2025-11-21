using System;

public class AbilityMechanic : IAbility
{
    public AbilityType Type { get; } = AbilityType.Mechanic;
    private GrenadesManager _grenadesManager;
    private bool _isAvailable = false; // По умолчанию у нас нет гранат
    public bool IsAvailable
    {
        get => _isAvailable;
        set
        {
            if (_isAvailable != value) OnAbilityAvailabilityChanged?.Invoke(value);
            _isAvailable = value;
        }
    }
    public AbilityUIData UIData { get; }
    public event Action<bool> OnAbilityAvailabilityChanged;

    public AbilityMechanic(GrenadesManager grenadesManager, AbilityUIData data)
    {
        _grenadesManager = grenadesManager;
        UIData = data;
    }

    public bool Try2ApplyAbility()
    {
        return _grenadesManager.ThrowGrenade();
    }

    public void CheckAreGrenadesAvailable(int count)
    {
        if (count > 0)
        {
            IsAvailable = true;
        }
        else
        {
            IsAvailable = false;
        }
    }
}
