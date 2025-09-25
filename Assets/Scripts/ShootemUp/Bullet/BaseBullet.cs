using UnityEngine;

public enum BulletType
{
    Regular,
    Hard,
    Explosive,
    Bouncing
}

public abstract class BaseBullet : MonoBehaviour, IBullet
{
    [Min(0.1f)]
    [SerializeField] private float speed;
    private Vector2 direction;
    [SerializeField] protected Damage damage;

    private bool isSpawnedByPLayer;
    public bool IsSpawnedByPlayer { get => isSpawnedByPLayer; }

    public float Speed => speed;
    public Vector2 Direction
    {
        get => direction;
        set
        {
            direction = value.normalized;
            RotateBullet(); // ѕоворачиваем при изменении направлени€
        }
    }

    protected Rigidbody2D _rb;

    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public virtual void Initialize(Vector2 startDirection, float bulletSpeed, bool spawnedByPlayer)
    {
        speed = bulletSpeed;
        Direction = startDirection.normalized;
        isSpawnedByPLayer = spawnedByPlayer;
    }

    private void RotateBullet()
    {
        if (direction != Vector2.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    public virtual void Initialize(Vector2 startDirection, bool spawnedByPlayer)
    {
        Direction = startDirection.normalized;
        isSpawnedByPLayer = spawnedByPlayer;
    }

    public virtual void Move()
    {
        _rb.velocity = Direction * Speed;
    }

    protected virtual void FixedUpdate()
    {
        Move();
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        // ≈сли подошли к краю - уничтожаемс€
        if (collision.transform.CompareTag("BulletDestroyer")) Destroy(gameObject);

        // ≈сли это друга€ пул€ - пропускаем
        if (collision.gameObject.TryGetComponent<IBullet>(out var _)) return;

        // ¬се пули так или иначе нанос€т урон

        // ѕровер€ем, можем ли мы нанести урон
        bool isPlayer = IsSpawnedByPlayer && collision.gameObject.TryGetComponent<PlayerMovementHandler>(out var _);
        if (collision.gameObject.TryGetComponent<IHealth>(out var entity))
        {
            // ≈сли коснулись игрока, и при этом пул€ заспавнена игроком - игнорируем
            if (isPlayer) return;
            entity.TakeDamage(damage);
        }
    }

    public override string ToString()
    {
        return $"I`m a bullet with direction {direction}, speed {speed}, and I`m {(IsSpawnedByPlayer ? " " : "not")}spawned by player";
    }
}