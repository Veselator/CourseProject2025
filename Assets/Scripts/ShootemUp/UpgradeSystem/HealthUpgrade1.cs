public class HealthUpgrade1 : IUpgrade
{
    public string MainText => "�������� ������'�";
    public string SecondText => "30%";
    public void ApplyUpgrade()
    {
        Health playerHealth = PlayerInstances.playerHealth;
        playerHealth.MaximumHealth *= 1.3f;
        playerHealth.CurrentHealth = playerHealth.MaximumHealth;
    }
}
