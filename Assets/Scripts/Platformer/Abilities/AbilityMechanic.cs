public class AbilityMechanic : IAbility
{
    private GrenadesManager _grenadesManager;
    public AbilityMechanic(GrenadesManager grenadesManager)
    {
        _grenadesManager = grenadesManager;
    }

    public void Try2ApplyAbility()
    {
        _grenadesManager.ThrowGrenade();
    }
}
