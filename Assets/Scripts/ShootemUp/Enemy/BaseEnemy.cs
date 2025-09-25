using UnityEngine;
using VContainer;

public abstract class BaseEnemy : MonoBehaviour, IEnemy
{
    [SerializeField] protected IHealth _healthTracker;
    [SerializeField] protected IMovement _movement;
    private IObjectResolver resolver;
    private BulletConfig _bulletConfig;

    // Экземпляр будет даже не знать, по какому паттерну он движется
    public IMovingPattern currentMovingPattern { get; set; }
    public float Health => _healthTracker.CurrentHealth;
    public bool IsDied => _healthTracker.IsDied;

    protected Vector2 currentTargetPosition;
    protected bool isMovingToTarget = false;
    protected float arrivalThreshold = 0.1f;

    [Inject]
    public void Construct(IObjectResolver resolver, BulletConfig bulletConfig)
    {
        this.resolver = resolver;
        this._bulletConfig = bulletConfig;
    }

    protected virtual void Start()
    {
        InitializeEnemy();
    }

    protected virtual void Update()
    {
        UpdateMovement();
    }

    protected virtual void Die()
    {
        DestroyEnemy();
    }

    protected virtual void InitializeEnemy()
    {
        _movement = GetComponent<IMovement>();
        _healthTracker = GetComponent<IHealth>();
        _healthTracker.OnDeath += Die;

        _movement.Init(Vector2.down);

        if (currentMovingPattern != null)
        {
            currentMovingPattern.Init();
            GetMovementVector();
        }
    }

    private void OnDestroy()
    {
        _healthTracker.OnDeath -= Die;
    }

    protected virtual void UpdateMovement()
    {
        if (currentMovingPattern == null || _movement == null) return;

        // Проверяем, достигли ли мы текущей цели
        if (!isMovingToTarget || Vector2.Distance(transform.position, currentTargetPosition) < arrivalThreshold)
        {
            GetMovementVector();
        }

        // Обновляем движение к текущей цели
        if (isMovingToTarget)
        {
            Vector2 direction = (currentTargetPosition - (Vector2)transform.position).normalized;
            _movement.ChangeVelocity(direction);
        }
    }

    protected void GetMovementVector()
    {
        if (currentMovingPattern != null)
        {
            Vector2 nextPosition = currentMovingPattern.GetNext();
            currentTargetPosition = nextPosition;
            isMovingToTarget = true;

            // Если паттерн завершен, продолжаем движение прямо вниз
            if (currentMovingPattern.IsCompleted)
            {
                HandlePatternCompleted();
            }
        }
    }

    protected virtual void HandlePatternCompleted()
    {
        // Базовое поведение - продолжить движение вниз за пределы экрана
        Vector2 bottomPosition = new Vector2(transform.position.x, -15f);
        currentTargetPosition = bottomPosition;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyDestroyer"))
        {
            DestroyEnemy();
        }

        if (other.CompareTag("Player"))
        {
            // При попадании в игрока
            DestroyEnemy();
        }
    }

    protected virtual void DestroyEnemy()
    {
        // Сообщаем, что мы умерли
        // НО
        // важно также учитывать характер смерти - от столкновения с игроком, от пули или от достижения границы
        WavesManager.OnEnemyDied?.Invoke();
        //GlobalFlags.ToggleFlag(GlobalFlags.Flags.SHOOTEMUP_ENEMY_DIED);
        // Здесь можно добавить эффекты уничтожения, дроп и т.д.
        Destroy(gameObject);
    }
}