using System;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestAction", menuName = "Quest/Action")]
public class QuestAction : ScriptableObject
{
    // Основа работы квеста - действие
    // Действием может быть загрузка уровня, воспроизведение анимации
    // или переход на другой экран
    public QuestActionEffect[] actionEffects;
}

[Serializable]
public struct QuestActionEffect
{
    public QuestEffectType effectType;
    public QuestConditionOperator conditionOperator;
    public QuestEffectCondition[] conditions;
    public float floatValue;
    [TextArea(3, 10)]
    public string stringValue;
    [Header("Для отправки сообщений И для PlayAnimationAtSpecificObject")]
    public string additionalStringValue;
}

public enum QuestEffectType
{
    NextLevel,
    HideObject,
    ShowObject,
    ChangeScreen,
    SetGlobalFlag,
    RemoveGlobalFlag,
    ToggleGlobalFlag,
    PlayAnimation, // Анимация на отправителе
    PlayAnimationAtSpecificObject,
    RemoveItem,
    ShowMessage, // Мысли главного героя
    InvokeMessage, // Отправляем сообщение определённому объекту
    InvokeMessageAtCurrentTarget,
    WinGame,
    StopTimer,
    ResumeTimer,
    HideUI,
    ShowUI,
    LoadSpecificLevel
}

[Serializable]
public struct QuestEffectCondition
{
    public QuestConditionType conditionType;
    public QuestEffectConditionModifier modifier;
    public string stringValue;
    public string additionalStringValue;
    public float floatValue;
}

public enum QuestEffectConditionModifier
{
    None,
    Not
}
public enum QuestConditionOperator
{
    Or,
    And
}

// Система условий
public enum QuestConditionType
{
    DoesHaveItem,
    IsGlobalFlag,
    GetBoolValueAtObject,
    IsGameObjectActive
}