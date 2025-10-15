using System;
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
    [TextArea(3, 10)]
    public string stringValue;
    [Header("��� �������� ��������� � ��� PlayAnimationAtSpecificObject")]
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
    PlayAnimation, // �������� �� �����������
    PlayAnimationAtSpecificObject,
    RemoveItem,
    ShowMessage, // ����� �������� �����
    InvokeMessage, // ���������� ��������� ������������ �������
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

// ������� �������
public enum QuestConditionType
{
    DoesHaveItem,
    IsGlobalFlag,
    GetBoolValueAtObject,
    IsGameObjectActive
}