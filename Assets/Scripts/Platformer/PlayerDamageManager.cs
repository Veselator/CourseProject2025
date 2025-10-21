using UnityEngine;

public class PlayerDamageManager : MonoBehaviour
{
    private IHealth _health;
    private RigidbodyPlatformerMovement _movement;

    [SerializeField] private LayerMask _enemyMask;
    [SerializeField] private Vector2 _bottomBox;
    [SerializeField] private Transform _bottomPointTransform;
    [SerializeField] private Damage PlayerDamageOnEnemies;
    [SerializeField] private float _impulseStrengthAfterDamageDealed = 2f;

    private void Start()
    {
        _health = GetComponent<Health>();
        _movement = GetComponent<RigidbodyPlatformerMovement>();
    }

    private bool IsPlayerAtTopOfEnemy(Vector2 enemyPosition)
    {
        return Physics2D.OverlapBox(_bottomPointTransform.position, _bottomBox, 0f, _enemyMask) && _bottomPointTransform.position.y > enemyPosition.y;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = UnityEngine.Color.red;
        Gizmos.DrawWireCube(_bottomPointTransform.position, _bottomBox);
    }

    private Vector2 GetNormalizedVectorBetween(Vector2 firstVector, Vector2 secondVector)
    {
        return (firstVector - secondVector).normalized;
    }

    private void OnCollisionEnter2D(Collision2D colli)
    {
        IPossible2DealDamage tempInterface;

        // Если не предмет, способный нанести урон - нам не интересно
        if (!colli.gameObject.TryGetComponent<IPossible2DealDamage>(out tempInterface)) return;
        BasePEnemy tempEnemy = tempInterface as BasePEnemy;

        if (tempEnemy)
        {
            // Это враг
            // Проверяем - мы наносим урон или по нам наносят урон
            if (IsPlayerAtTopOfEnemy(colli.transform.position))
            {
                tempEnemy.Health.TakeDamage(PlayerDamageOnEnemies);
            }
            else
            {
                // Урон получает игрок
                // Плак-плак
                _health.TakeDamage(tempEnemy.DealedDamage);
            }
        }
        else
        {
            // Очевидно, что это статичное препятствие
            // При всём нашем большом желании мы ему не сможем ничего навредить
            // А он нам - сможет
            Debug.Log($"WOW! I touched static obstacle! Amazing!");
            _health.TakeDamage(tempInterface.DealedDamage);
        }

        _movement.DoImpulse(GetNormalizedVectorBetween(transform.position, colli.transform.position), _impulseStrengthAfterDamageDealed);
    }
}
