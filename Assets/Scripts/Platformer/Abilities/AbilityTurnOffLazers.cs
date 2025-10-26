using System;

public class AbilityTurnOffLazers : IAbility
{
    private LaserTurnOffer _laserTurnOffer;
    private bool _isAvailable = true;
    public float duration = 4f;
    private float timeAfterDuration = 2f;
    public float CooldownTime;

    public bool IsAvailable
    {
        get => _isAvailable;
        set
        {
            if(_isAvailable != value) OnAbilityAvailabilityChanged?.Invoke(value);
            _isAvailable = value;
        }
    }

    public AbilityUIData UIData { get; }
    public event Action<bool> OnAbilityAvailabilityChanged;

    public AbilityTurnOffLazers(LaserTurnOffer laserTurnOffer, AbilityUIData data)
    {
        _laserTurnOffer = laserTurnOffer;
        CooldownTime = duration + timeAfterDuration;
        UIData = data;
    }

    public void Try2ApplyAbility()
    {
        if (!_isAvailable) return;

        _laserTurnOffer.StartCoroutineForTurinngOffLasers(this, duration, timeAfterDuration);
        IsAvailable = false;
    }
}
