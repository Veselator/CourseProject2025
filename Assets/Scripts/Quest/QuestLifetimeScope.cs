using VContainer;
using VContainer.Unity;
using UnityEngine;

public class QuestLifetimeScope : LifetimeScope
{
    [SerializeField] private QuestActionProccessor _actionProccessor;
    [SerializeField] private QuestAnimationManager _animationManager;
    [SerializeField] private QuestInventoryManager _questInventoryManager;
    [SerializeField] private QuestCameraManager _cameraManager;
    [SerializeField] private QuestTimerManager _timerManager;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(_actionProccessor).AsSelf();
        builder.RegisterInstance(_animationManager).AsSelf();
        builder.RegisterInstance(_questInventoryManager).AsSelf();
        builder.RegisterInstance(_cameraManager).AsSelf();
        builder.RegisterInstance(_timerManager).AsSelf();

        builder.RegisterComponentInHierarchy<QuestActionProccessor>();
        builder.RegisterComponentInHierarchy<QuestAnimationManager>();
        builder.RegisterComponentInHierarchy<QuestInventoryManager>();
        builder.RegisterComponentInHierarchy<QuestCameraManager>();
        builder.RegisterComponentInHierarchy<QuestTimerManager>();
    }
}
