// Интерфейс абилки
using System;

public interface IAbility
{
    // В будуем можно добавить публичное свойство LinkedUIData (на базе Scriptable object)
    // для отображения абилки в UI
    bool IsAvailable { get; set; }
    AbilityType Type { get; }
    AbilityUIData UIData { get; }

    event Action<bool> OnAbilityAvailabilityChanged;
    void Try2ApplyAbility();
}

// Для идентификации для анимаций
public enum AbilityType
{
    Mechanic,
    SolvePuzzles,
    StrongPunch,
    TurnOffLasers
}