using System;
using UnityEngine;

public class PlayerDamageManager : MonoBehaviour
{
    private IHealth _health;
    public IHealth Health => _health;
    private RigidbodyPlatformerMovement _movement;
    private InvulnerabilityManager _invulnerabilityManager;

    [SerializeField] private LayerMask _enemyMask;
    [SerializeField] private Vector2 _bottomBox;
    [SerializeField] private Transform _bottomPointTransform;
    [SerializeField] private Damage PlayerDamageOnEnemies;
    [SerializeField] private float _impulseStrengthAfterDamageDealed = 2f;

    public event Action OnPlayerDamaged;
    private void Start()
    {
        _health = GetComponent<Health>();
        _movement = GetComponent<RigidbodyPlatformerMovement>();
        _invulnerabilityManager = GetComponent<InvulnerabilityManager>();
    }

    private bool IsPlayerAtTopOfEnemy(Vector2 enemyPosition)
    {
        return Physics2D.OverlapBox(_bottomPointTransform.position, _bottomBox, 0f, _enemyMask) && _bottomPointTransform.position.y > enemyPosition.y;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
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
        Laser laser = tempInterface as Laser;

        if (!_invulnerabilityManager.IsInvulnerable) // Если неуязвимы - то урона не будет, хоть и игрока всё равно оттолкнёт
        {
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
                    _invulnerabilityManager.ResetTimer();
                }
            }
            else if (laser)
            {
                // Усё
                // Допрыгался фраер
                _health.TakeDamage(laser.DealedDamage);
                _invulnerabilityManager.ResetTimer();
            }
            else
            {
                // Очевидно, что это статичное препятствие
                // При всём нашем большом желании мы ему не сможем никак навредить
                // А он нам - сможет
                _health.TakeDamage(tempInterface.DealedDamage);
                _invulnerabilityManager.ResetTimer();
            }
        }

        Vector2 contactPoint = Vector2.zero;
        for (int i = 0; i < colli.contactCount; i++)
        {
            contactPoint += colli.GetContact(i).point;
        }
        contactPoint /= colli.contactCount;

        Vector2 knockbackDirection = GetNormalizedVectorBetween((Vector2)transform.position, contactPoint);
        Debug.Log($"Knockback direction is {knockbackDirection}, _impulseStrengthAfterDamageDealed is {_impulseStrengthAfterDamageDealed}");

        _movement.DoImpulse(knockbackDirection, _impulseStrengthAfterDamageDealed);
        OnPlayerDamaged?.Invoke();
    }
}
