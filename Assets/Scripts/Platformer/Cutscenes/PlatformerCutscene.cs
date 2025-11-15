using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Cutscene", menuName = "Platformer/Cutscene")]
public class PlatformerCutscene : ScriptableObject
{
    // Что такое кат-сцена? Последовательность определённых действий

    public CutsceneAction[] Actions;
}

[Serializable]
public struct CutsceneAction
{
    public CutsceneActionType CAT;
    public WaitActionType WaitType;

    [ShowIfEnum("CAT", CutsceneActionType.SetCameraTrackingObject, CutsceneActionType.SetPlayerSpawnpoint, CutsceneActionType.MoveObject)]
    public string linkedObjectName; // Да, это не очень хорошо, но более правильное решение потребует больше времени

    [ShowIfEnum("CAT", CutsceneActionType.SetCameraSize)]
    public float Size;

    [ShowIfEnum("CAT", CutsceneActionType.StartDialogue)]
    public DialogueSO LinkedDialogue;

    [ShowIfEnum("CAT", CutsceneActionType.MoveObject)]
    public string destinationObjectName;

    [ShowIfEnum("CAT", CutsceneActionType.MoveObject)]
    public bool animateScale;

    [ShowIfEnum("CAT", CutsceneActionType.MoveObject)]
    public float animationDuration;

    [ShowIfEnum("WaitType", WaitActionType.WaitForSeconds)]
    public float TimeInSeconds;

    [ShowIfEnum("CAT", CutsceneActionType.SetBossEmotion)]
    public BossEmotion emotion;

    [ShowIfEnum("CAT", CutsceneActionType.PlaySpecificMusic)]
    public string musicName;
}

public enum CutsceneActionType
{
    None,

    // Движение игрока
    BlockMovement,
    UnblockMovement,
    ToggleMovement,

    // Камера
    SetCameraTrackingObject,
    ReturnCameraToPlayer,
    SetCameraSize,
    ResetCameraSize,

    // Диалог
    StartDialogue,

    // Босс
    BossActionAfterCutscene,
    
    // Другое
    SetPlayerSpawnpoint,

    // Перемещение объекта
    MoveObject,

    // Эмоции босса
    SetBossEmotion,

    // Музыка
    PlaySpecificMusic
}

public enum WaitActionType
{
    None,
    WaitForSeconds,
    WaitForEndOfTheAction // Работает только с диалогами
}
