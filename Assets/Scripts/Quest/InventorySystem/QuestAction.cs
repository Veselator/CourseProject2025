using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestAction", menuName = "Quest/Action")]
public class QuestAction : ScriptableObject
{
    // ������ ������ ������ - ��������
    // ��������� ����� ���� �������� ������, ��������������� ��������
    // ��� ������� �� ������ �����
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

// ������� �������
public enum QuestConditionType
{
    DoesHaveItem,
    IsGlobalFlag
}