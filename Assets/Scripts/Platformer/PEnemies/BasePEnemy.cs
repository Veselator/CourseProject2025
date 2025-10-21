using UnityEngine;

public abstract class BasePEnemy : MonoBehaviour, IPossible2DealDamage
{
    private IHealth _health;
    [SerializeField] private Damage dealedDamage;
    public Damage DealedDamage => dealedDamage;
    public IHealth Health => _health;
    protected IMovement _movement;

    protected virtual void Start()
    {
        InitComponents();
    }

    private void InitComponents()
    {
        _health = GetComponent<IHealth>();
        _movement = GetComponent<IMovement>();
    }
}
