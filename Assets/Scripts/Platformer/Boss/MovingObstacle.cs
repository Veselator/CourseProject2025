using UnityEngine;

public class MovingObstacle : MovingPlatform, IPossible2DealDamage
{
    [SerializeField] private Damage dealedDamage;
    public Damage DealedDamage => dealedDamage;
    // Используется для босса
    private PlatformerObstaclesSpawner _linkedSpawner;
    public void Init(PlatformerObstaclesSpawner spawner, float newSpeed, Transform fromPoint, Transform toPoint)
    {
        _linkedSpawner = spawner;
        speed = newSpeed;
        points = new Transform[2] { fromPoint, toPoint };
        _isLooped = false;
    }

    protected override void Start()
    {
        base.Start();
        OnMovementEnded += EndMovement;
    }

    private void EndMovement()
    {
        OnMovementEnded -= EndMovement;
        _linkedSpawner.HandleObstacleDestroyed();
        Destroy(gameObject);
    }
}
