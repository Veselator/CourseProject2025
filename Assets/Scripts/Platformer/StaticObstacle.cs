using UnityEngine;

public class StaticObstacle : MonoBehaviour, IPossible2DealDamage
{
    [SerializeField] private Damage dealedDamage;
    public Damage DealedDamage => dealedDamage;
}
