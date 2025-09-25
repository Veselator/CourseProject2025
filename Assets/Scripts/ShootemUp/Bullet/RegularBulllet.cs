using UnityEngine;

public class RegularBullet : BaseBullet
{
    [SerializeField] private float lifetime = 5f;

    protected override void Awake()
    {
        base.Awake();
        Destroy(gameObject, lifetime);
    }

    protected new void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        // Если коснулись игрока, и при этом пуля заспавнена игроком - игнорируем
        if (IsSpawnedByPlayer && collision.gameObject.TryGetComponent<PlayerMovementHandler>(out var _)) return;

        // Уничтожаем пулю при любом столкновении
        Destroy(gameObject);
    }
}