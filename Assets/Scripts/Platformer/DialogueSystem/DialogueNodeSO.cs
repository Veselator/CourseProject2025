using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueNodeSO", menuName = "Platformer/Dialogue/DialogueNodeSO")]
public class DialogueNodeSO : ScriptableObject
{
    // Реплика
    public DialogueCharacterSO Character;
    [TextArea(3, 7)]
    public string Text;
    public CharacterTypingSpeedConfig[] TypingConfig;
    public float DelayAfter;
}

// Гибкая система настройки скорости ввода
// Для разных частей - разная скорость
[Serializable]
public struct CharacterTypingSpeedConfig
{
    public int IDTo; // Включительно
    public float TypingSpeed;
    public float DelayAfter;
}