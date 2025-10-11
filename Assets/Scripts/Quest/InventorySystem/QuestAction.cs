using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestAction", menuName = "Quest/Action")]
public class QuestAction : ScriptableObject
{
    // Основа работы квеста - действие
    public QuestActionEffect[] actionEffects;
}

public struct QuestActionEffect
{
    public ActionType actionType;
    public float value;
    public GameObject target;
}

public enum ActionType
{
    LoadLevel,
    HideObject,
    ShowObject,
    ChangeScreen
}