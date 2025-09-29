using UnityEngine;
using VContainer;

public abstract class BaseEnemy : MonoBehaviour, IEnemy
{
    [SerializeField] protected IHealth _healthTracker;
    [SerializeField] protected IMovement _movement;
    [SerializeField] private GameObject boomParticle;

    [SerializeField] private int scoreAdd;
    private ShootemUpScoreManager _scoreManager;

    // ��������� ����� ���� �� �����, �� ������ �������� �� ��������
    public IMovingPattern currentMovingPattern { get; set; }
    public float Health => _healthTracker.CurrentHealth;
    public bool IsDied => _healthTracker.IsDied;

    protected Vector2 currentTargetPosition;
    protected bool isMovingToTarget = false;
    protected float arrivalThreshold = 0.1f;

    [SerializeField] private Damage damageOnCollisionWithPlayer;

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
        Instantiate(boomParticle, gameObject.transform.position, Quaternion.identity);
        Debug.Log("Enemy just died");
        DestroyEnemy();
    }

    protected virtual void InitializeEnemy()
    {
        _movement = GetComponent<IMovement>();
        _healthTracker = GetComponent<IHealth>();
        _scoreManager = ShootemUpScoreManager.Instance;

        _healthTracker.OnDeath += Die;
        _healthTracker.OnDeath += AddScore;

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
        _healthTracker.OnDeath -= AddScore;
    }

    private void AddScore()
    {
        _scoreManager.AddScoreAction?.Invoke(scoreAdd);
    }

    protected virtual void UpdateMovement()
    {
        if (currentMovingPattern == null || _movement == null) return;

        // ���������, �������� �� �� ������� ����
        if (!isMovingToTarget || Vector2.Distance(transform.position, currentTargetPosition) < arrivalThreshold)
        {
            GetMovementVector();
        }

        // ��������� �������� � ������� ����
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

            // ���� ������� ��������, ���������� �������� ����� ����
            if (currentMovingPattern.IsCompleted)
            {
                HandlePatternCompleted();
            }
        }
    }

    protected virtual void HandlePatternCompleted()
    {
        // ������� ��������� - ���������� �������� ���� �� ������� ������
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
            // ��� ��������� � ������
            other.GetComponent<IHealth>().TakeDamage(damageOnCollisionWithPlayer);
            Die();
        }
    }

    protected virtual void DestroyEnemy()
    {
        // ��������, ��� �� ������
        // ��
        // ����� ����� ��������� �������� ������ - �� ������������ � �������, �� ���� ��� �� ���������� �������
        Debug.Log("Enemy just destroyed");
        WavesManager.OnEnemyDied?.Invoke();
        //GlobalFlags.ToggleFlag(GlobalFlags.Flags.SHOOTEMUP_ENEMY_DIED);
        // ����� ����� �������� ������� �����������, ���� � �.�.
        Destroy(gameObject);
    }
}