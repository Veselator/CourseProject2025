using System;

public class AbilitySolvePuzzles : IAbility
{
    public AbilityType Type { get; } = AbilityType.SolvePuzzles;
    // Не удивляйся - эта абилка просто есть
    public bool IsAvailable { get; set; } = true;
    public AbilityUIData UIData { get; }
    public event Action<bool> OnAbilityAvailabilityChanged;
    public AbilitySolvePuzzles(AbilityUIData data) { UIData = data; }

    public void Try2ApplyAbility() { }
}
