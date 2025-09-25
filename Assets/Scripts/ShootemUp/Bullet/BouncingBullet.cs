using UnityEngine;

public class BouncingBullet : BaseBullet
{
    [SerializeField] private int maxNumOfReflections;
    private int currentNumOfReflections = 0;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        // ≈сли подошли к краю - уничтожаемс€
        if (collision.transform.CompareTag("BulletDestroyer")) Destroy(gameObject);

        // ≈сли это друга€ пул€ - пропускаем
        if (collision.gameObject.TryGetComponent<IBullet>(out var _)) return;

        // ¬се пули так или иначе нанос€т урон

        // ѕровер€ем, можем ли мы нанести урон
        if (collision.gameObject.TryGetComponent<IHealth>(out var entity))
        {
            // ≈сли коснулись игрока, и при этом пул€ заспавнена игроком - игнорируем
            bool isPlayer = collision.gameObject.TryGetComponent<PlayerMovementHandler>(out var _);

            if (isPlayer) OnCollidedWithPlayer(entity);
            else OnCollidedWithEnemy(entity);

            

        }
    }

    protected override void OnCollidedWithPlayer(IHealth entity)
    {
        // ≈сли пул€ коснулась игрока и заспавнена игроком - ничего
        if (IsSpawnedByPlayer) return;
        Hit(entity);

        // TODO: логика отражени€!
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
        // ѕростой способ - направление от центра коллайдера к пуле
        return (transform.position - hitCollider.transform.position).normalized;
    }

    protected override void Hit(IHealth entity)
    {
        base.Hit(entity);
    }
}
