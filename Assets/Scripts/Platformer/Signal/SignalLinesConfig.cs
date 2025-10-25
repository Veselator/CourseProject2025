using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SignalLinesConfig", menuName = "Platformer/SignalLinesConfig")]
public class SignalLinesConfig : ScriptableObject
{
    // Конфиг для настройки головоломки с проводами
    public SignalLineConfig[] signals;
    public SignalDirection startDirection;
    [Header("Правильное решение срабатывает только один раз?")]
    public bool IsOneShoot;
}

[Serializable]
public struct SignalLineConfig
{
    public Signal signal;
    public SignalDirection direction;
}
