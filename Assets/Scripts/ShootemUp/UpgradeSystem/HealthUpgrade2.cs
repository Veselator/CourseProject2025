public class HealthUpgrade2 : IUpgrade
{
    public string MainText => "«б≥льшити здоров'€";
    public string SecondText => "50%";
    public void ApplyUpgrade()
    {
        Health playerHealth = PlayerInstances.playerHealth;
        playerHealth.MaximumHealth *= 1.5f;
        playerHealth.CurrentHealth = playerHealth.MaximumHealth;
    }
}
