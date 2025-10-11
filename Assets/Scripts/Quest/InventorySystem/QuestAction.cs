using System;
using System.Collections;
using System.Collections.Generic;
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
    public string stringValue;
}

public enum QuestEffectType
{
    NextLevel,
    HideObject,
    ShowObject,
    ChangeScreen,
    SetGlobalFlag,
    PlayAnimation
}

[Serializable]
public struct QuestEffectCondition
{
    public QuestConditionType conditionType;
    public QuestEffectConditionModifier modifier;
    public string stringValue;
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
    IsGlobalFlag
}