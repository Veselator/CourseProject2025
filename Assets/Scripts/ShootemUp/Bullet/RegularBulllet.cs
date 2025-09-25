using UnityEngine;

public class RegularBullet : BaseBullet
{
    [SerializeField] private float lifetime = 5f;

    protected override void Awake()
    {
        base.Awake();
        Destroy(gameObject, lifetime);
    }
}