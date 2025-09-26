using UnityEngine;

public enum BulletType
{
    Regular,
    AntiArmor,
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
    public GameObject Instance => this.gameObject;
    public float Speed => speed;
    public Vector2 Direction
    {
        get => direction;
        set
        {
            direction = value.normalized;
            RotateBullet(); // Поворачиваем при изменении направления
        }
    }

    protected Rigidbody2D _rb;

    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public virtual void Initialize(Vector2 startDirection, float bulletSpeed, bool spawnedByPlayer, float DamageMultiplayer)
    {
        speed = bulletSpeed;
        Direction = startDirection.normalized;
        isSpawnedByPLayer = spawnedByPlayer;
        damage.damageMultiplier = DamageMultiplayer;
    }

    public virtual void Initialize(Vector2 startDirection, bool spawnedByPlayer, float DamageMultiplayer)
    {
        Initialize(startDirection, speed, spawnedByPlayer, DamageMultiplayer);
    }

    public virtual void Initialize(Vector2 startDirection, bool spawnedByPlayer)
    {
        Initialize(startDirection, speed, spawnedByPlayer, 1f);
    }

    private void RotateBullet()
    {
        if (direction != Vector2.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    public virtual void Move()
    {
        _rb.velocity = Direction * Speed;
    }

    protected virtual void FixedUpdate()
    {
        Move();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        // Если подошли к краю - уничтожаемся
        if (collision.transform.CompareTag("BulletDestroyer")) Destroy(gameObject);

        // Если это другая пуля - пропускаем
        if (collision.gameObject.TryGetComponent<IBullet>(out var _)) return;

        // Все пули так или иначе наносят урон

        // Проверяем, можем ли мы нанести урон
        if (collision.gameObject.TryGetComponent<IHealth>(out var entity))
        {
            // Если коснулись игрока, и при этом пуля заспавнена игроком - игнорируем
            bool isPlayer = collision.gameObject.TryGetComponent<PlayerMovementHandler>(out var _);

            if (isPlayer) OnCollidedWithPlayer(entity);
            else OnCollidedWithEnemy(entity);
        }
    }

    protected virtual void OnCollidedWithPlayer(IHealth entity)
    {
        // Если пуля коснулась игрока и заспавнена игроком - ничего
        if (IsSpawnedByPlayer) return;
        Hit(entity);
        // В любом случае уничтожаем после контакта
        Destroy(gameObject);
    }

    protected virtual void OnCollidedWithEnemy(IHealth entity)
    {
        if (!IsSpawnedByPlayer) return;
        Hit(entity);
        Destroy(gameObject);
    }

    protected virtual void Hit(IHealth entity)
    {
        entity.TakeDamage(damage);
    }

    protected virtual void OnDestroy()
    {
        // Какие-то эффекты прописать по умолчанию
    }

    public override string ToString()
    {
        return $"I`m a bullet with direction {direction}, speed {speed}, and I`m {(IsSpawnedByPlayer ? "" : "not ")}spawned by player";
    }
}