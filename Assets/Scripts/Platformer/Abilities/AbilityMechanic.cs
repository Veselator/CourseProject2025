public class AbilityMechanic : IAbility
{
    private GrenadesManager _grenadesManager;
    public bool IsAvailable { get; set; } = true;

    public AbilityMechanic(GrenadesManager grenadesManager)
    {
        _grenadesManager = grenadesManager;
    }

    public void Try2ApplyAbility()
    {
        _grenadesManager.ThrowGrenade();
    }
}
