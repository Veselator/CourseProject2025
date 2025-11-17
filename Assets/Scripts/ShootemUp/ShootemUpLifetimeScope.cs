using VContainer;
using VContainer.Unity;
using UnityEngine;

public class ShootemUpLifetimeScope : LifetimeScope
{
    [SerializeField] public EnemyConfig _enemyConfig;
    [SerializeField] public BulletConfig _bulletConfig;
    [SerializeField] public WavesStreamConfig _wavesStreamConfig;
    [SerializeField] public CameraShake _cameraShake;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(_enemyConfig).AsSelf();
        builder.RegisterInstance(_bulletConfig).AsSelf();
        builder.RegisterInstance(_wavesStreamConfig).AsSelf();
        builder.RegisterInstance(_cameraShake).AsSelf();

        builder.RegisterComponentInHierarchy<WavesManager>();
        builder.RegisterComponentInHierarchy<EnemySpawner>();
    }
}
