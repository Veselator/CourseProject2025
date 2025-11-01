using System;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] private float explodeTimer = 4f;
    [SerializeField] private float explosionRadius = 2f;
    [SerializeField] private float explosivePhysicalPower = 50f;
    [SerializeField] private float explosivePowerScaler = 0.87f;
    [SerializeField] private Damage explosionDamage;

    public event Action OnExlode;

    private void Update()
    {
        HoldTimer();
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawSphere(transform.position, explosionRadius);
    //}

    private void HoldTimer()
    {
        explodeTimer -= Time.deltaTime;
        if (explodeTimer <= 0f)
        {
            Explode();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        BasePEnemy tempEnemy;
        if (!collision.gameObject.TryGetComponent<BasePEnemy>(out tempEnemy)) return;

        Explode();
    }

    private void Explode()
    {
        // Boom!
        OnExlode?.Invoke(); // Для визуала

        IHealth tempHealth;
        Rigidbody2D tempRigidbody;
        Collider2D[] collidersInZoneOfExplosion = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D colli in collidersInZoneOfExplosion)
        {
            // Отниманем урон
            if (colli.gameObject.TryGetComponent<IHealth>(out tempHealth)) tempHealth.TakeDamage(explosionDamage);
            // Физически показываем 
            if (colli.gameObject.TryGetComponent<Rigidbody2D>(out tempRigidbody))
            {
                Vector2 direction2ExplosiveCenter = (Vector2)(colli.transform.position - transform.position).normalized;
                // Вот тут внимательно
                // Умножаем на tempRigidbody.mass что-бы невелировать значение массы и объекты одинаково красиво разлетались
                // ПРИ ЭТОМ умножаем на explosivePowerScaler, т.к. значение tempRigidbody.mass может быть достаточно большим
                tempRigidbody.AddForce(explosivePhysicalPower * tempRigidbody.mass * explosivePowerScaler * direction2ExplosiveCenter, ForceMode2D.Impulse);
            }
        }

        Destroy(gameObject);
    }
}
