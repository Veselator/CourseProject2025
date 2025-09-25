using UnityEngine;

public class BouncingBullet : BaseBullet
{
    [SerializeField] private int maxNumOfReflections;
    private int currentNumOfReflections = 0;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        // ���� ������� � ���� - ������������
        if (collision.transform.CompareTag("BulletDestroyer")) Destroy(gameObject);

        // ���� ��� ������ ���� - ����������
        if (collision.gameObject.TryGetComponent<IBullet>(out var _)) return;

        // ��� ���� ��� ��� ����� ������� ����

        // ���������, ����� �� �� ������� ����
        if (collision.gameObject.TryGetComponent<IHealth>(out var entity))
        {
            // ���� ��������� ������, � ��� ���� ���� ���������� ������� - ����������
            bool isPlayer = collision.gameObject.TryGetComponent<PlayerMovementHandler>(out var _);

            if (isPlayer) OnCollidedWithPlayer(entity);
            else OnCollidedWithEnemy(entity);

            

        }
    }

    protected override void OnCollidedWithPlayer(IHealth entity)
    {
        // ���� ���� ��������� ������ � ���������� ������� - ������
        if (IsSpawnedByPlayer) return;
        Hit(entity);

        // TODO: ������ ���������!
    }

    protected override void OnCollidedWithEnemy(IHealth entity)
    {
        if (!IsSpawnedByPlayer) return;
        Hit(entity);
    }

    private void Reflect(Collider2D collider)
    {
        currentNumOfReflections++;
        Vector2 normal = GetSurfaceNormal(collider);
        Direction = Vector2.Reflect(Direction, normal);
    }

    private Vector2 GetSurfaceNormal(Collider2D hitCollider)
    {
        // ������� ������ - ����������� �� ������ ���������� � ����
        return (transform.position - hitCollider.transform.position).normalized;
    }

    protected override void Hit(IHealth entity)
    {
        base.Hit(entity);
    }
}
