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

    [ShowIfEnum("CAT", CutsceneActionType.SetCameraTrackingObject)]
    public string linkedObjectName; // Да, это не очень хорошо, но более правильное решение потребует больше времени

    [ShowIfEnum("CAT", CutsceneActionType.SetCameraSize)]
    public float Size;

    [ShowIfEnum("CAT", CutsceneActionType.StartDialogue)]
    public DialogueSO LinkedDialogue;

    [ShowIfEnum("WaitType", WaitActionType.WaitForSeconds)]
    public float TimeInSeconds;
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
    BossActionAfterCutscene
}

public enum WaitActionType
{
    None,
    WaitForSeconds,
    WaitForEndOfTheAction // Работает только с диалогами
}
