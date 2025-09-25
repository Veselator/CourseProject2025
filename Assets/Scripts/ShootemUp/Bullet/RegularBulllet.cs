using UnityEngine;

public class RegularBullet : BaseBullet
{
    [SerializeField] private float lifetime = 5f;

    protected override void Awake()
    {
        base.Awake();
        Destroy(gameObject, lifetime);
    }

    protected override void OnCollidedWithPlayer(IHealth entity)
    {
        if (IsSpawnedByPlayer) return;
        entity.TakeDamage(damage);
        Destroy(gameObject);
    }

    protected override void OnCollidedWithEnemy(IHealth entity)
    {
        if (!IsSpawnedByPlayer) return;
        entity.TakeDamage(damage);
        Destroy(gameObject);
    }
}