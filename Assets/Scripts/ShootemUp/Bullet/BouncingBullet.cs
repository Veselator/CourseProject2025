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
        Reflect(entity);
    }

    protected override void OnCollidedWithEnemy(IHealth entity)
    {
        if (!IsSpawnedByPlayer) return;
        Hit(entity);
        Reflect(entity);
    }

    private void Reflect(IHealth entity)
    {
        if (currentNumOfReflections > maxNumOfReflections)
        {
            Destroy(gameObject);
            return;
        }

        Debug.Log($"BULLET REFLECTED! old direction = {Direction}");
        currentNumOfReflections++;

        float minAngle = -45f;
        float maxAngle = 45f;

        Vector2 normal = new Vector2(0, -1);
        Vector2 reflected = Vector2.Reflect(Direction, normal).normalized;

        float randomAngle = Random.Range(minAngle, maxAngle);
        Vector2 newDirection = Quaternion.Euler(0, 0, randomAngle) * reflected;
        Direction = newDirection.normalized;

        damage.damageMultiplier *= 0.7f;
        Debug.Log($"                 normal: {normal} new Direction = {Direction} (angle offset: {randomAngle})");
    }

    protected override void Hit(IHealth entity)
    {
        base.Hit(entity);
    }
}
