using UnityEngine;

public class ExplosiveBullet : BaseBullet
{
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private float explosionRadius = 2f;
    [SerializeField] private LayerMask targetLayerMask = -1; // ��� ����
    private bool isExploded = false;

    protected override void Awake()
    {
        base.Awake();
        Destroy(gameObject, lifetime);
    }

    protected override void Hit(IHealth entity)
    {
        entity.TakeDamage(damage);
        Explode();
    }

    private void Explode()
    {
        isExploded = true;
        // �������� ��� ���������� � ������� ������
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, targetLayerMask);

        foreach (var hitCollider in hitColliders)
        {
            // ���������, ���� �� ��������� IHealth
            if (hitCollider.TryGetComponent<IHealth>(out var healthComponent))
            {
                // ���������, ��� ��� �� ��� �� ������, �������� �� ��� ������� ����
                // � ��������� ������ ��� ������/����������
                bool isPlayer = hitCollider.TryGetComponent<PlayerMovementHandler>(out var _);

                if (isPlayer && IsSpawnedByPlayer || !IsSpawnedByPlayer) continue;

                // ������� ���� �� ������
                healthComponent.TakeDamage(damage);
            }
        }
    }

    protected override void OnDestroy()
    {
        if (!isExploded) Explode();
    }
}